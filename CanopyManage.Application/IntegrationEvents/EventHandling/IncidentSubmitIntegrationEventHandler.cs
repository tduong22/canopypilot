using CanopyManage.Application.Commands.SubmitIncident;
using CanopyManage.Application.IntegrationEvents.Events;
using CanopyManage.Common.EventBus.Abstractions;
using MediatR;
using System;
using System.Threading.Tasks;

namespace CanopyManage.Application.IntegrationEvents.EventHandling
{
    /// <summary>
    /// The Handler which handle the alert message comming from Azure Service Bus Topic
    /// </summary>
    public class IncidentSubmitIntegrationEventHandler : IIntegrationEventHandler<IncidentSubmitIntegrationEvent>
    {
        private readonly IMediator mediator;

        public IncidentSubmitIntegrationEventHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task Handle(IncidentSubmitIntegrationEvent @event)
        {
            var submitIncidentCommand = new SubmitAlertIncidentCommand()
            {
                AlertId = @event.AlertId,
                AlertType  = @event.AlertType,
                ServiceNowSettingID = @event.ServiceNowSettingID,
                Title = @event.Title,
                Message = @event.Message
            };

            await mediator.Send(submitIncidentCommand);
        }
    }
}
