using CanopyManage.Application.IntegrationEvents.Events;
using CanopyManage.Application.Services;
using CanopyManage.Application.Services.Requests;
using CanopyManage.Application.Services.Responses;
using CanopyManage.Common.EventBus.Abstractions;
using CanopyManage.Domain.Entities;
using CanopyManage.Domain.SeedWork;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CanopyManage.Application.Commands.SubmitIncident
{
    public class SubmitAlertIncidentCommandHandler : IRequestHandler<SubmitAlertIncidentCommand>
    {
        private readonly IServiceNowService _serviceNowService;
        private readonly IEventBusQueuePublisher _eventBusQueuePublisher;
        private readonly IRepository<ServiceNowServiceAccount, string> _accountRepository;
        private readonly ILogger<SubmitAlertIncidentCommandHandler> _logger;

        public SubmitAlertIncidentCommandHandler(IServiceNowService serviceNowService,
                                                 IEventBusQueuePublisher eventBusQueuePublisher,
                                                 IRepository<ServiceNowServiceAccount, string> accountRepository,
                                                 ILogger<SubmitAlertIncidentCommandHandler> logger)
        {
            _serviceNowService = serviceNowService ?? throw new ArgumentNullException(nameof(serviceNowService));
            _eventBusQueuePublisher = eventBusQueuePublisher ?? throw new ArgumentNullException(nameof(eventBusQueuePublisher));
            _accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger)); ;
        }

        public async Task<Unit> Handle(SubmitAlertIncidentCommand request, CancellationToken cancellationToken)
        {
            var serviceNowAccount = await RetrieveServiceNowServiceAccountAsync(request, cancellationToken);

            var addNewIncidentRequest = new AddNewIncidentRequest()
            {
                TenantId = request.TenantId,
                Title = request.Title,
                Message = request.Message,
                AlertId = request.AlertId
            };

            _logger.LogInformation($"{nameof(SubmitAlertIncidentCommandHandler)} - Submitting user with tenant {request.TenantId} and Service Now setting id {request.ServiceNowSettingID}");
            //submit incident to ServiceNow API
            AddNewIncidentResponse result = await _serviceNowService.AddNewIncidentAsync(serviceNowAccount.ServiceUserName, serviceNowAccount.ServiceSecret, addNewIncidentRequest, cancellationToken);

            var incidentSubmittedResult = new IncidentSubmittedResultIntegrationEvent()
            {
                AlertId = result.Result.AlertId,
                ResponseCode = result.ResponseCode,
                Message = result.Result.Message
            };

            await _eventBusQueuePublisher.PublishAsync(incidentSubmittedResult);
            return Unit.Value;
        }

        private async Task<ServiceNowServiceAccount> RetrieveServiceNowServiceAccountAsync(SubmitAlertIncidentCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{nameof(SubmitAlertIncidentCommandHandler)} - Retrieving user with tenant {request.TenantId} and Service Now setting id {request.ServiceNowSettingID}");
            //Retrieve user credentitals
            var serviceNowAccount = await _accountRepository.GetByIdAsync(
                ServiceNowServiceAccount.ComposeServiceNowServiceAccountId(request.TenantId, request.ServiceNowSettingID),
                cancellationToken);

            return serviceNowAccount;
        }
    }
}
