using CanopyManage.Domain.Aggregates.Incidents.Events;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CanopyManage.Application.DomainEventHandlers
{
    public class IncidentSubmittedDomainEventHandler : INotificationHandler<IncidentSubmittedDomainEvent>
    {
        public Task Handle(IncidentSubmittedDomainEvent notification, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
