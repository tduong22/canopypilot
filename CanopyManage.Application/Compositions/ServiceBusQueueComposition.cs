using Autofac;
using CanopyManage.Common.EventBus.Abstractions;
using CanopyManage.Common.EventBusServiceBus;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CanopyManage.Application.Compositions
{
    public static class ServiceBusQueueComposition
    {
        public static IServiceCollection AddQueueResponsePublisher(this IServiceCollection services, string sbConnection)
        {
            services.AddSingleton<IEventBusQueuePublisher, ServiceBusPublisher>(sp =>
            {
                var serviceBusConnection = new ServiceBusConnectionStringBuilder(sbConnection);
                var serviceBusPersisterConnection = new DefaultServiceBusPersisterConnection(serviceBusConnection, EntityType.Queue);
                var iLifetimeScope = sp.GetRequiredService<ILifetimeScope>();
                var logger = sp.GetRequiredService<ILogger<ServiceBusSubscriber>>();
                var env = sp.GetRequiredService<IHostingEnvironment>();

                return new ServiceBusPublisher(serviceBusPersisterConnection, env.EnvironmentName);
            });

            return services;
        }
    }
}
