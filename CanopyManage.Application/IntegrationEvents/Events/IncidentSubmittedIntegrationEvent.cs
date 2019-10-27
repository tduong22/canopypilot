using CanopyManage.Common.EventBus.Events;

namespace CanopyManage.Application.IntegrationEvents.Events
{
    public class IncidentSubmittedResultIntegrationEvent : ResultIntegrationEvent
    {
        public string AlertId { get; set; }
    }
}
