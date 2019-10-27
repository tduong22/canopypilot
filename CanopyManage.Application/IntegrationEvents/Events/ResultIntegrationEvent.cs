using CanopyManage.Common.EventBus.Events;

namespace CanopyManage.Application.IntegrationEvents.Events
{
    public class ResultIntegrationEvent : IntegrationEvent
    {
        public string ResponseCode { get; set; }
        public string Message { get; set; }
    }
}
