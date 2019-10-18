using CanopyManage.Common.EventBus.Events;
using System.Threading.Tasks;

namespace CanopyManage.Common.EventBus.Abstractions
{
    public interface IEventBusSubscriber
    {
        Task SubscribeAsync<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>;

        Task UnsubscribeAsync<T, TH>()
            where TH : IIntegrationEventHandler<T>
            where T : IntegrationEvent;
    }
}
