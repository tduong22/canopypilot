﻿using CanopyManage.Common.EventBus.Abstractions;
using CanopyManage.Common.EventBusServiceBus;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CanopyManage.Application.Compositions
{
    public static class EventBusPublisher
    {
        public static IServiceCollection AddEventBusPublisher(this IServiceCollection services, string sbConnection, string environment)
        {
            services.AddSingleton<IServiceBusPersisterConnection>(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<DefaultServiceBusPersisterConnection>>();

                var serviceBusConnection = new ServiceBusConnectionStringBuilder(sbConnection);

                return new DefaultServiceBusPersisterConnection(serviceBusConnection, logger);
            });

            services.AddSingleton<IEventBusPublisher, ServiceBusPublisher>(sp =>
            {
                var serviceBusPersisterConnection = sp.GetRequiredService<IServiceBusPersisterConnection>();
                var logger = sp.GetRequiredService<ILogger<ServiceBusPublisher>>();

                return new ServiceBusPublisher(serviceBusPersisterConnection, logger, environment);
            });

            return services;
        }
    }
}