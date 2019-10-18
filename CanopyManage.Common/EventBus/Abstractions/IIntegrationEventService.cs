using CanopyManage.Common.EventBus.Events;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CanopyManage.Common.EventBus.Abstractions
{
    public interface IIntegrationEventService
    {
        Task PublishEventsAsync(IntegrationEvent evt);

        Task PublishEventsAsync(IList<IntegrationEvent> evt);
    }
}
