using CanopyManage.Common.EventBus.Events;

namespace CanopyManage.Application.IntegrationEvents.Events
{
    public class IncidentSubmittedResultIntegrationEvent : IntegrationEvent
    {
        public string AlertId { get; set; }
        public string ResponseCode {get;set; }
        public string Message { get; set; }
    }

}
