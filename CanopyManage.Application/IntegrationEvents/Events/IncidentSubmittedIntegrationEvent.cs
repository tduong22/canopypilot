using CanopyManage.Common.EventBus.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace CanopyManage.Application.IntegrationEvents.Events
{
    public class IncidentSubmittedIntegrationEvent : IntegrationEvent
    {
        public string AlertId { get; set; }
        public int ServiceNowSettingID { get; set; }
        public string AlertType { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
    }
}
