using Autofac;
using CanopyManage.Common.EventBus;
using CanopyManage.Common.EventBus.Abstractions;
using CanopyManage.Common.EventBusServiceBus;
using CanopyManage.Common.EventBusServiceBus.Options;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CanopyManage.IncidentService.Compositions
{
    public static class EventBusComposition
    {
        public static IServiceCollection AddEventBusSubscriber(this IServiceCollection services, string sbConnection, string subscriptionName)
        {
            services.AddSingleton<IServiceBusPersisterConnection>(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<DefaultServiceBusPersisterConnection>>();

                var serviceBusConnection = new ServiceBusConnectionStringBuilder(sbConnection);

                return new DefaultServiceBusPersisterConnection(serviceBusConnection, logger);
            });

            services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();
            services.AddSingleton<IEventBusSubscriber, ServiceBusSubscriber>(sp =>
            {
                var serviceBusPersisterConnection = sp.GetRequiredService<IServiceBusPersisterConnection>();
                var iLifetimeScope = sp.GetRequiredService<ILifetimeScope>();
                var logger = sp.GetRequiredService<ILogger<ServiceBusSubscriber>>();
                var env = sp.GetRequiredService<IHostingEnvironment>();

                var option = new ServiceBusOption
                {
                    SubscriptionName = subscriptionName,
                    CheckSubscriptionExist = true,
                    SubscriptionRequireSession = false
                };

                return new ServiceBusSubscriber(serviceBusPersisterConnection, logger,
                       iLifetimeScope, option, env.EnvironmentName);
            });

            return services;
        }
    }
}
