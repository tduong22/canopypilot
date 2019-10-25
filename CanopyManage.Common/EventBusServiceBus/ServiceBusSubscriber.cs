namespace CanopyManage.Common.EventBusServiceBus
{
    using Autofac;
    using CanopyManage.Common.EventBus;
    using CanopyManage.Common.EventBus.Abstractions;
    using CanopyManage.Common.EventBus.Events;
    using CanopyManage.Common.EventBusServiceBus.Options;
    using Microsoft.Azure.ServiceBus;
    using Microsoft.Azure.ServiceBus.Management;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    public class ServiceBusSubscriber : IEventBusSubscriber
    {
        private readonly IServiceBusPersisterConnection _serviceBusPersisterConnection;
        private readonly ILogger<ServiceBusSubscriber> _logger;
        private readonly IEventBusSubscriptionsManager _subsManager;
        private readonly SubscriptionClient _subscriptionClient;
        private readonly ILifetimeScope _autofac;
        private readonly string AUTOFAC_SCOPE_NAME = "ServiceBusSubscriberScope";
        private readonly ServiceBusOption _serviceBusOption;
        public string INTEGRATION_EVENT_SUFIX = "IntegrationEvent";
        public readonly string _eventSourceSystem = "SOURCE_SYSTEM";
        public const string SOURCE_SYSTEM_KEY = "SOURCE_SYSTEM";

        public ServiceBusSubscriber(
            IServiceBusPersisterConnection serviceBusPersisterConnection,
            ILogger<ServiceBusSubscriber> logger,
            ILifetimeScope autofac,
            ServiceBusOption serviceBusOption,
            string eventSourceSystem)
        {
            _serviceBusOption = serviceBusOption ?? throw new ArgumentNullException(nameof(serviceBusOption));
            _serviceBusPersisterConnection = serviceBusPersisterConnection ?? throw new ArgumentNullException(nameof(serviceBusPersisterConnection));
            _eventSourceSystem = eventSourceSystem ?? throw new ArgumentNullException(nameof(eventSourceSystem));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _autofac = autofac;

            if (_serviceBusOption.CheckSubscriptionExist)
            {
                CreateSubscriptionIfNotExistsAsync().GetAwaiter().GetResult();
            }

            _subsManager = new InMemoryEventBusSubscriptionsManager();

            _subscriptionClient = new SubscriptionClient(serviceBusPersisterConnection.ServiceBusConnectionStringBuilder,
                serviceBusOption.SubscriptionName);
            _subscriptionClient.PrefetchCount = serviceBusOption.PrefetchCount;
            RemoveDefaultRule();
            RegisterSubscriptionClientMessageHandler();
        }

        private void RemoveDefaultRule()
        {
            try
            {
                var rules = _subscriptionClient.GetRulesAsync().GetAwaiter().GetResult();
                if (rules.Select(x => x.Name).Contains(RuleDescription.DefaultRuleName))
                    _subscriptionClient
                     .RemoveRuleAsync(RuleDescription.DefaultRuleName)
                     .GetAwaiter()
                     .GetResult();
            }
            catch (MessagingEntityNotFoundException)
            {
                _logger.LogInformation($"The messaging entity {RuleDescription.DefaultRuleName} Could not be found.");
            }
        }

        private async Task CreateSubscriptionIfNotExistsAsync()
        {
            var connectionStringBuilder = _serviceBusPersisterConnection.ServiceBusConnectionStringBuilder;
            var topicName = connectionStringBuilder.EntityPath;

            var client = new ManagementClient(connectionStringBuilder.GetEntityConnectionString());
            if (!await client.SubscriptionExistsAsync(topicName, _serviceBusOption.SubscriptionName))
            {
                var topicSub = new SubscriptionDescription(topicName, _serviceBusOption.SubscriptionName)
                {
                    RequiresSession = _serviceBusOption.SubscriptionRequireSession
                };

                try
                {
                    await client.CreateSubscriptionAsync(topicSub);
                }
                catch (MessagingEntityAlreadyExistsException)
                {
                    _logger.LogInformation($"The subscription ${_serviceBusOption.SubscriptionName} already exists.");
                }
            }
        }

        public async Task SubscribeAsync<T, TH>(IDictionary<string, object> filterProperties = null, CancellationToken cancellationToken = default(CancellationToken))
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {
            var eventName = typeof(T).Name.Replace(INTEGRATION_EVENT_SUFIX, "");
            var filter = new CorrelationFilter()
            {
                Label = eventName
            };

            if (filterProperties != null)
            {
                foreach (var item in filterProperties)
                {
                    filter.Properties.Add(item);
                }
            }

            if (_serviceBusOption.SubscriptionRequireSession && !string.IsNullOrEmpty(_serviceBusOption.SessionIdFilter))
            {
                filter.SessionId = _serviceBusOption.SessionIdFilter;
            }

            var ruleDescription = new RuleDescription
            {
                Filter = filter,
                Name = eventName
            };

            if (_serviceBusOption.CustomFilters != null)
            {
                foreach (var property in _serviceBusOption.CustomFilters)
                {
                    filter.Properties.Add(property.Key, property.Value);
                }
            }

            var containsKey = _subsManager.HasSubscriptionsForEvent<T>();
            if (!containsKey)
            {
                try
                {
                    var filters = await _subscriptionClient.GetRulesAsync();
                    await _subscriptionClient.AddRuleAsync(ruleDescription);
                }
                catch (ServiceBusException)
                {
                    Console.WriteLine($"The messaging entity {eventName} already exists.");
                }
            }

            _subsManager.AddSubscription<T, TH>();
        }

        public async Task UnsubscribeAsync<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {
            var eventName = typeof(T).Name.Replace(INTEGRATION_EVENT_SUFIX, "");

            try
            {
                await _subscriptionClient.RemoveRuleAsync(eventName);
            }
            catch (MessagingEntityNotFoundException)
            {
                _logger.LogInformation($"The messaging entity {eventName} Could not be found.");
            }

            _subsManager.RemoveSubscription<T, TH>();
        }

        private void RegisterSubscriptionClientMessageHandler()
        {
            if (_serviceBusOption.SubscriptionRequireSession)
            {
                _subscriptionClient.RegisterSessionHandler(
                    async (session, message, token) =>
                    {
                        if (IsEligebleToProcess(message))
                        {
                            var eventName = $"{message.Label}{INTEGRATION_EVENT_SUFIX}";
                            var messageData = Encoding.UTF8.GetString(message.Body);

                            // Complete the message so that it is not received again.
                            await session.CompleteAsync(message.SystemProperties.LockToken);

                            await ProcessEvent(eventName, messageData);
                        }
                        else
                        {
                            await session.AbandonAsync(message.SystemProperties.LockToken);
                        }
                    },
                    new SessionHandlerOptions(ExceptionReceivedHandler)
                    {
                        MaxConcurrentSessions = _serviceBusOption.MaxConcurrentSessions,
                        AutoComplete = _serviceBusOption.AutoCompleted,
                        MaxAutoRenewDuration = TimeSpan.FromMinutes(1)
                    });
            }
            else
            {
                _subscriptionClient.RegisterMessageHandler(
                    async (message, token) =>
                    {
                        if (IsEligebleToProcess(message))
                        {
                            var eventName = $"{message.Label}{INTEGRATION_EVENT_SUFIX}";
                            var messageData = Encoding.UTF8.GetString(message.Body);

                            // Complete the message so that it is not received again.
                            await _subscriptionClient.CompleteAsync(message.SystemProperties.LockToken);

                            await ProcessEvent(eventName, messageData);
                        }
                        else
                        {
                            await _subscriptionClient.AbandonAsync(message.SystemProperties.LockToken);
                        }
                    },
                    new MessageHandlerOptions(ExceptionReceivedHandler)
                    {
                        MaxConcurrentCalls = _serviceBusOption.MaxConcurrentSessions,
                        AutoComplete = _serviceBusOption.AutoCompleted,
                        MaxAutoRenewDuration = TimeSpan.FromMinutes(1)
                    });
            }
        }

        private Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            Console.WriteLine($"Message handler encountered an exception {exceptionReceivedEventArgs.Exception}.");
            var context = exceptionReceivedEventArgs.ExceptionReceivedContext;
            Console.WriteLine("Exception context for troubleshooting:");
            Console.WriteLine($"- Endpoint: {context.Endpoint}");
            Console.WriteLine($"- Entity Path: {context.EntityPath}");
            Console.WriteLine($"- Executing Action: {context.Action}");
            return Task.CompletedTask;
        }

        private bool IsEligebleToProcess(Message message)
        {
            try
            {
                //Validate source system
                string sourceSystem = message.UserProperties[SOURCE_SYSTEM_KEY]?.ToString();
                return sourceSystem.Equals(_eventSourceSystem, StringComparison.OrdinalIgnoreCase);
            }
            catch (Exception)
            {
                return false;
            }
        }

        private async Task<bool> ProcessEvent(string eventName, string message)
        {
            if (!_subsManager.HasSubscriptionsForEvent(eventName))
                return false;

            using (var scope = _autofac.BeginLifetimeScope(AUTOFAC_SCOPE_NAME))
            {
                var subscriptions = _subsManager.GetHandlersForEvent(eventName);
                foreach (var subscription in subscriptions)
                {
                    var handler = scope.ResolveOptional(subscription.HandlerType);
                    if (handler == null) continue;
                    var eventType = _subsManager.GetEventTypeByName(eventName);
                    var integrationEvent = JsonConvert.DeserializeObject(message, eventType);
                    var concreteType = typeof(IIntegrationEventHandler<>).MakeGenericType(eventType);
                    await (Task)concreteType.GetMethod("Handle").Invoke(handler, new object[] { integrationEvent });
                }
                return true;
            }
        }

        public void Dispose()
        {
            _subsManager.Clear();
        }
    }
}