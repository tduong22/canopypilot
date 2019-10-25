using CanopyManage.Common.EventBus.Events;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CanopyManage.Common.EventBus.Abstractions
{
    public interface IEventBusSubscriber
    {
        Task SubscribeAsync<T, TH>(IDictionary<string,object> filterProperties = null, CancellationToken cancellationToken = default(CancellationToken))
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>;

        Task UnsubscribeAsync<T, TH>()
            where TH : IIntegrationEventHandler<T>
            where T : IntegrationEvent;
    }
}
