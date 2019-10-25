using CanopyManage.Application.IntegrationEvents.Events;
using CanopyManage.Application.Services;
using CanopyManage.Application.Services.Requests;
using CanopyManage.Application.Services.Responses;
using CanopyManage.Common.EventBus.Abstractions;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CanopyManage.Application.Commands.SubmitIncident
{
    public class SubmitIncidentCommandHandler : IRequestHandler<SubmitIncidentCommand>
    {
        private readonly IServiceNowService _serviceNowService;
        private readonly IEventBusQueuePublisher _eventBusQueuePublisher;

        public SubmitIncidentCommandHandler(IServiceNowService serviceNowService, IEventBusQueuePublisher eventBusQueuePublisher)
        {
            _serviceNowService = serviceNowService ?? throw new ArgumentNullException(nameof(serviceNowService));
            _eventBusQueuePublisher = eventBusQueuePublisher ?? throw new ArgumentNullException(nameof(eventBusQueuePublisher));
        }

        public async Task<Unit> Handle(SubmitIncidentCommand request, CancellationToken cancellationToken)
        {
            var addNewIncidentRequest = new AddNewIncidentRequest()
            {
                Title = request.Title,
                Message = request.Message
            };

            string username = "admin";
            string password = "Password1";
            AddNewIncidentResponse result = await _serviceNowService.AddNewIncidentAsync(username, password, addNewIncidentRequest, cancellationToken);

            await _eventBusQueuePublisher.PublishAsync(new IncidentSubmittedIntegrationEvent()
            {
                AlertId = result.Result.AlertId,
                ResponseCode = result.ResponseCode,
                Message = result.Result.Message
            });
            return Unit.Value;
        }
    }
}
