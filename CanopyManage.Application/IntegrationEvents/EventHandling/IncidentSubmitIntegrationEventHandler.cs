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
        private readonly IEventBusQueuePublisher _eventBusQueuePublisher;

        public IncidentSubmitIntegrationEventHandler(IMediator mediator, IEventBusQueuePublisher eventBusQueuePublisher)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _eventBusQueuePublisher = eventBusQueuePublisher ?? throw new ArgumentNullException(nameof(eventBusQueuePublisher));
        }

        public async Task Handle(IncidentSubmitIntegrationEvent @event)
        {
            try
            {
                var submitIncidentCommand = new SubmitAlertIncidentCommand()
                {
                    AlertId = @event.AlertId,
                    AlertType = @event.AlertType,
                    ServiceNowSettingID = @event.ServiceNowSettingID,
                    Title = @event.Title,
                    Message = @event.Message
                };

                await mediator.Send(submitIncidentCommand);
            }
            catch (Exception ex)
            {
                var errorEvent = new IncidentSubmittedResultIntegrationEvent()
                {
                    AlertId = @event.AlertId,
                    ResponseCode = "500",
                    Message = ex.Message
                };

                await _eventBusQueuePublisher.PublishAsync(errorEvent);
            }
        }
    }
}
