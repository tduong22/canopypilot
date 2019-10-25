using CanopyManage.Application.Commands.SubmitIncident;
using CanopyManage.Application.IntegrationEvents.Events;
using CanopyManage.Common.EventBus.Abstractions;
using MediatR;
using System;
using System.Threading.Tasks;

namespace CanopyManage.Application.IntegrationEvents.EventHandling
{
    public class IncidentSubmitIntegrationEventHandler : IIntegrationEventHandler<IncidentSubmitIntegrationEvent>
    {
        private readonly IMediator mediator;

        public IncidentSubmitIntegrationEventHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task Handle(IncidentSubmitIntegrationEvent @event)
        {
            var submitIncidentCommand = new SubmitIncidentCommand()
            {
                ServiceNowSettingID = @event.ServiceNowSettingID,
                Title = @event.Title,
                Message = @event.Message,
                AlertId = @event.AlertId,
                AlertType = @event.AlertType
            };

            await mediator.Send(submitIncidentCommand);
        }
    }
}
