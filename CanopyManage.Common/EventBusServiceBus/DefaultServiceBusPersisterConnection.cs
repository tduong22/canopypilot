using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;
using System;

namespace CanopyManage.Common.EventBusServiceBus
{
    public class DefaultServiceBusPersisterConnection : IServiceBusPersisterConnection
    {
        private EntityType _entityType;
        private ISenderClient _senderClient;
        private bool _disposed;

        public DefaultServiceBusPersisterConnection(ServiceBusConnectionStringBuilder serviceBusConnectionStringBuilder, EntityType entityType)
        {
            ServiceBusConnectionStringBuilder = serviceBusConnectionStringBuilder ?? 
                throw new ArgumentNullException(nameof(serviceBusConnectionStringBuilder));
            
            _entityType = entityType;
            
            _senderClient = entityType == EntityType.Topic
                ? new TopicClient(ServiceBusConnectionStringBuilder, RetryPolicy.Default)
                : (ISenderClient)new QueueClient(ServiceBusConnectionStringBuilder, ReceiveMode.PeekLock, RetryPolicy.Default);
        }

        public ServiceBusConnectionStringBuilder ServiceBusConnectionStringBuilder { get; }

        public ISenderClient CreateModel()
        {
            if(_senderClient.IsClosedOrClosing)
            {
                _senderClient = _entityType == EntityType.Topic
                 ? new TopicClient(ServiceBusConnectionStringBuilder, RetryPolicy.Default)
                 : (ISenderClient)new QueueClient(ServiceBusConnectionStringBuilder, ReceiveMode.PeekLock, RetryPolicy.Default);
            }
            return _senderClient;
        }

        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;
        }
    }
}
