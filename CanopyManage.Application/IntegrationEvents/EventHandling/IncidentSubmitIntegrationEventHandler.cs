using CanopyManage.Application.Commands.SubmitIncident;
using CanopyManage.Application.IntegrationEvents.Events;
using CanopyManage.Common.EventBus.Abstractions;
using FluentValidation;
using MediatR;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CanopyManage.Application.IntegrationEvents.EventHandling
{
    /// <summary>
    /// The Handler which handle the alert message comming from Azure Service Bus Topic
    /// </summary>
    public class IncidentSubmitIntegrationEventHandler : IIntegrationEventHandler<IncidentSubmitIntegrationEvent>
    {
        private readonly IMediator _mediator;
        private readonly IEventBusQueuePublisher _eventBusQueuePublisher;

        public IncidentSubmitIntegrationEventHandler(IMediator mediator, IEventBusQueuePublisher eventBusQueuePublisher)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _eventBusQueuePublisher = eventBusQueuePublisher ?? throw new ArgumentNullException(nameof(eventBusQueuePublisher));
        }

        public async Task Handle(IncidentSubmitIntegrationEvent @event)
        {
            try
            {
                var submitIncidentCommand = new SubmitAlertIncidentCommand()
                {
                    TenantId = @event.TenantId,
                    AlertId = @event.AlertId,
                    AlertType = @event.AlertType,
                    ServiceNowSettingID = @event.ServiceNowSettingID,
                    Title = @event.Title,
                    Message = @event.Message
                };

                await _mediator.Send(submitIncidentCommand);
            }
            catch (ValidationException valEx)
            {
                //This could be moved to an implementation of an implementation ValidatorBehavior for this specific pipeline
                var errorFields = valEx.Errors.Select(x=>x.ToString());
                var valErrorEvent = new IncidentSubmittedResultIntegrationEvent()
                {
                    AlertId = @event.AlertId,
                    ResponseCode = "400",
                    Message = valEx.Message + String.Join(",", errorFields)
                };
                await _eventBusQueuePublisher.PublishAsync(valErrorEvent);
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
