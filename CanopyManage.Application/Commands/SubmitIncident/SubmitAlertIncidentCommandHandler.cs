﻿using CanopyManage.Application.IntegrationEvents.Events;
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
    public class SubmitAlertIncidentCommandHandler : IRequestHandler<SubmitAlertIncidentCommand>
    {
        private readonly IServiceNowService _serviceNowService;
        private readonly IEventBusQueuePublisher _eventBusQueuePublisher;

        public SubmitAlertIncidentCommandHandler(IServiceNowService serviceNowService, IEventBusQueuePublisher eventBusQueuePublisher
            )
        {
            _serviceNowService = serviceNowService ?? throw new ArgumentNullException(nameof(serviceNowService));
            _eventBusQueuePublisher = eventBusQueuePublisher ?? throw new ArgumentNullException(nameof(eventBusQueuePublisher));
        }

        public async Task<Unit> Handle(SubmitAlertIncidentCommand request, CancellationToken cancellationToken)
        {
            IncidentSubmittedIntegrationEvent incidentSubmittedEvent;
            try
            {
                var addNewIncidentRequest = new AddNewIncidentRequest()
                {
                    Title = request.Title,
                    Message = request.Message,
                    AlertId = request.AlertId
                };
                //Retrieve user credentitals
                string username = "admin";
                string password = "Password1";
                AddNewIncidentResponse result = await _serviceNowService.AddNewIncidentAsync(username, password, addNewIncidentRequest, cancellationToken);

                incidentSubmittedEvent = new IncidentSubmittedIntegrationEvent()
                {
                    AlertId = result.Result.AlertId,
                    ResponseCode = result.ResponseCode,
                    Message = result.Result.Message
                };
            }
            catch (Exception ex)
            {
                incidentSubmittedEvent = new IncidentSubmittedIntegrationEvent()
                {
                    AlertId = request.AlertId,
                    ResponseCode = "500",
                    Message = ex.Message
                };
            }
            await _eventBusQueuePublisher.PublishAsync(incidentSubmittedEvent);
            return Unit.Value;
        }
    }
}