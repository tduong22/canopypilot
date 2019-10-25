using Autofac;
using CanopyManage.Common.EventBus;
using CanopyManage.Common.EventBus.Abstractions;
using CanopyManage.Common.EventBusServiceBus;
using CanopyManage.Common.EventBusServiceBus.Options;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CanopyManage.IncidentService.Compositions
{
    public static class EventBusSubscriberComposition
    {
        public static IServiceCollection AddEventBusSubscriber(this IServiceCollection services, string sbConnection, string subscriptionName, string environmentName)
        {
            services.AddSingleton<IServiceBusPersisterConnection>(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<DefaultServiceBusPersisterConnection>>();

                var serviceBusConnection = new ServiceBusConnectionStringBuilder(sbConnection);
                return new DefaultServiceBusPersisterConnection(serviceBusConnection, EntityType.Topic);
            });

            services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();
            
            services.AddSingleton<IEventBusSubscriber, ServiceBusSubscriber>(sp =>
            {
                var serviceBusPersisterConnection = sp.GetRequiredService<IServiceBusPersisterConnection>();
                var iLifetimeScope = sp.GetRequiredService<ILifetimeScope>();
                var logger = sp.GetRequiredService<ILogger<ServiceBusSubscriber>>();

                var option = new ServiceBusOption
                {
                    SubscriptionName = subscriptionName,
                    CheckSubscriptionExist = true,
                    SubscriptionRequireSession = false
                };

                return new ServiceBusSubscriber(serviceBusPersisterConnection, logger,
                       iLifetimeScope, option, environmentName);
            });

            return services;
        }
    }
}
