using Autofac;
using CanopyManage.Common.EventBus;
using CanopyManage.Common.EventBus.Abstractions;
using CanopyManage.Common.EventBusServiceBus;
using CanopyManage.Common.EventBusServiceBus.Options;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CanopyManage.WebService.Compositions
{
    public static class EventBusComposition
    {
        public static IServiceCollection AddEventBus(this IServiceCollection services)
        {
            services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();

            services.AddSingleton<IServiceBusPersisterConnection>(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<DefaultServiceBusPersisterConnection>>();

                var serviceBusConnection = new ServiceBusConnectionStringBuilder("EventBusConnection");

                return new DefaultServiceBusPersisterConnection(serviceBusConnection, EntityType.Topic);
            });

            services.AddSingleton<IEventBusPublisher, ServiceBusPublisher>(sp =>
            {
                var serviceBusPersisterConnection = sp.GetRequiredService<IServiceBusPersisterConnection>();
                var logger = sp.GetRequiredService<ILogger<ServiceBusPublisher>>();

                return new ServiceBusPublisher(serviceBusPersisterConnection, "");
            });

            services.AddSingleton<IEventBusSubscriber, ServiceBusSubscriber>(sp =>
            {
                var serviceBusPersisterConnection = sp.GetRequiredService<IServiceBusPersisterConnection>();
                var iLifetimeScope = sp.GetRequiredService<ILifetimeScope>();
                var logger = sp.GetRequiredService<ILogger<ServiceBusSubscriber>>();

                var option = new ServiceBusOption
                {
                    SubscriptionName = "SubscriptionClientName",
                    CheckSubscriptionExist = true,
                    SubscriptionRequireSession = false
                };

                return new ServiceBusSubscriber(serviceBusPersisterConnection, logger,
                       iLifetimeScope, option, "EventSourceSystem");
            });

            return services;
        }
    }
}
