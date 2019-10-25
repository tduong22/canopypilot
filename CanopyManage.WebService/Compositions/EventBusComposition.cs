using Autofac;
using CanopyManage.Common.EventBus;
using CanopyManage.Common.EventBus.Abstractions;
using CanopyManage.Common.EventBusServiceBus;
using CanopyManage.Common.EventBusServiceBus.Options;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CanopyManage.WebService.Compositions
{
    public static class EventBusComposition
    {
        public static IServiceCollection AddEventBus(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();

            services.AddSingleton<IServiceBusPersisterConnection>(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<DefaultServiceBusPersisterConnection>>();

                var serviceBusConnection = new ServiceBusConnectionStringBuilder(configuration["EventBus:TopicConnection"]);

                return new DefaultServiceBusPersisterConnection(serviceBusConnection, EntityType.Topic);
            });

            services.AddSingleton<IEventBusPublisher, ServiceBusPublisher>(sp =>
            {
                var serviceBusPersisterConnection = sp.GetRequiredService<IServiceBusPersisterConnection>();
                var logger = sp.GetRequiredService<ILogger<ServiceBusPublisher>>();
                var env = sp.GetRequiredService<IHostingEnvironment>();

                return new ServiceBusPublisher(serviceBusPersisterConnection, env.EnvironmentName);
            });

            services.AddSingleton<IEventBusSubscriber, ServiceBusSubscriber>(sp =>
            {
                var serviceBusPersisterConnection = sp.GetRequiredService<IServiceBusPersisterConnection>();
                var iLifetimeScope = sp.GetRequiredService<ILifetimeScope>();
                var logger = sp.GetRequiredService<ILogger<ServiceBusSubscriber>>();
                var env = sp.GetRequiredService<IHostingEnvironment>();

                var option = new ServiceBusOption
                {
                    SubscriptionName = $"SubscriptionClientName {env.EnvironmentName}",
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
