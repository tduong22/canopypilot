using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace CanopyManage.Domain.Aggregates.Incidents.Events
{
    public class IncidentSubmittedDomainEvent : INotification
    {
        public string AlertId { get; set; }
        public string ResponseCode { get; set; }
        public string Message { get; set; }
    }
}
