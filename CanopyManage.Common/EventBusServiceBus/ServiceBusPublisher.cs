using CanopyManage.Common.EventBus.Abstractions;
using CanopyManage.Common.EventBus.Events;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanopyManage.Common.EventBusServiceBus
{
    public class ServiceBusPublisher : IEventBusPublisher, IEventBusTopicPublisher, IEventBusQueuePublisher
    {
        private readonly IServiceBusPersisterConnection _serviceBusPersisterConnection;
        
        public string INTEGRATION_EVENT_SUFIX = "IntegrationEvent";
        public readonly string _eventSourceSystem = "SOURCE_SYSTEM";
        public const string SOURCE_SYSTEM_KEY = "SOURCE_SYSTEM";

        public ServiceBusPublisher(IServiceBusPersisterConnection serviceBusPersisterConnection,
                                   string eventSourceSystem)
        {
            _serviceBusPersisterConnection = serviceBusPersisterConnection ?? throw new ArgumentNullException(nameof(serviceBusPersisterConnection));
            _eventSourceSystem = eventSourceSystem ?? throw new ArgumentNullException(nameof(eventSourceSystem));
        }


        private Message ConvertToMessage(Event @event, IDictionary<string, string> userDictionary = null, string partitionKey = "")
        {
            var eventName = @event.GetType().Name.Replace(INTEGRATION_EVENT_SUFIX, "");
            var jsonMessage = JsonConvert.SerializeObject(@event);
            var body = Encoding.UTF8.GetBytes(jsonMessage);

            var message = new Message
            {
                MessageId = Guid.NewGuid().ToString(),
                Body = body,
                Label = eventName
            };

            if (!string.IsNullOrEmpty(partitionKey))
                message.SessionId = partitionKey;

            message.UserProperties.Add(SOURCE_SYSTEM_KEY, _eventSourceSystem);

            if (userDictionary != null)
            {
                foreach (var property in userDictionary)
                {
                    message.UserProperties.Add(property.Key, property.Value);
                }
            }

            return message;
        }

        public async Task PublishAsync(Event @event, string partitionKey = "", IDictionary<string, string> userDictionary = null)
        {
            var message = ConvertToMessage(@event, userDictionary, partitionKey);
            var senderClient = _serviceBusPersisterConnection.CreateModel();
            await senderClient.SendAsync(message);
        }

        public async Task PublishAsync(IEnumerable<Event> eventList, string partitionKey = "", IDictionary<string, string> userDictionary = null)
        {
            if (eventList == null || !eventList.Any())
                return;
            var messageList = eventList.Select(@event => ConvertToMessage(@event, userDictionary, partitionKey)).ToList();
            var senderClient = _serviceBusPersisterConnection.CreateModel();
            await senderClient.SendAsync(messageList);
        }
    }
}