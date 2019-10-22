using CanopyManage.Application.Commands.SubmitIncident;
using CanopyManage.Application.IntegrationEvents.Events;
using CanopyManage.Common.EventBus.Abstractions;
using MediatR;
using System;
using System.Threading.Tasks;

namespace CanopyManage.Application.IntegrationEvents.EventHandling
{
    public class IncidentSubmittedIntegrationEventHandler : IIntegrationEventHandler<IncidentSubmittedIntegrationEvent>
    {
        private readonly IMediator mediator;

        public IncidentSubmittedIntegrationEventHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task Handle(IncidentSubmittedIntegrationEvent @event)
        {
            var submitIncidentCommand = new SubmitIncidentCommand()
            {
                Title = @event.Title,
                Message = @event.Message
            };

            await mediator.Send(submitIncidentCommand);
        }
    }
}
