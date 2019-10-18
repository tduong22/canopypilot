using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Logging;
using System;

namespace CanopyManage.Common.EventBusServiceBus
{
    public class DefaultServiceBusPersisterConnection :IServiceBusPersisterConnection
    {
        private readonly ILogger<DefaultServiceBusPersisterConnection> _logger;
        private ITopicClient _topicClient;

        bool _disposed;

        public DefaultServiceBusPersisterConnection(ServiceBusConnectionStringBuilder serviceBusConnectionStringBuilder,
            ILogger<DefaultServiceBusPersisterConnection> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            
            ServiceBusConnectionStringBuilder = serviceBusConnectionStringBuilder ?? 
                throw new ArgumentNullException(nameof(serviceBusConnectionStringBuilder));
            _topicClient = new TopicClient(ServiceBusConnectionStringBuilder, RetryPolicy.Default);
        }

        public ServiceBusConnectionStringBuilder ServiceBusConnectionStringBuilder { get; }

        public ITopicClient CreateModel()
        {
            if(_topicClient.IsClosedOrClosing)
            {
                _topicClient = new TopicClient(ServiceBusConnectionStringBuilder, RetryPolicy.Default);
            }

            return _topicClient;
        }

        public void Dispose()
        {
            if (_disposed) return;

            _disposed = true;
        }
    }
}
