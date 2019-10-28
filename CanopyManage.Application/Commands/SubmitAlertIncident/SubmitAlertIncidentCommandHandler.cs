using CanopyManage.Application.IntegrationEvents.Events;
using CanopyManage.Application.Services;
using CanopyManage.Application.Services.Requests;
using CanopyManage.Application.Services.Responses;
using CanopyManage.Common.EventBus.Abstractions;
using CanopyManage.Domain.Entities;
using CanopyManage.Domain.SeedWork;
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
        private readonly IRepository<ServiceNowServiceAccount, string> _accountRepository;

        public SubmitAlertIncidentCommandHandler(IServiceNowService serviceNowService,
                                                 IEventBusQueuePublisher eventBusQueuePublisher,
                                                 IRepository<ServiceNowServiceAccount, string> accountRepository)
        {
            _serviceNowService = serviceNowService ?? throw new ArgumentNullException(nameof(serviceNowService));
            _eventBusQueuePublisher = eventBusQueuePublisher ?? throw new ArgumentNullException(nameof(eventBusQueuePublisher));
            _accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository)); ;
        }

        public async Task<Unit> Handle(SubmitAlertIncidentCommand request, CancellationToken cancellationToken)
        {
            var addNewIncidentRequest = new AddNewIncidentRequest()
            {
                TenantId = request.TenantId,
                Title = request.Title,
                Message = request.Message,
                AlertId = request.AlertId
            };

            //Retrieve user credentitals
            var serviceNowAccount = await _accountRepository.GetByIdAsync(
                ServiceNowServiceAccount.ComposeServiceNowServiceAccountId(request.TenantId, request.ServiceNowSettingID),
                cancellationToken);

            string username = serviceNowAccount.ServiceUserName;
            string password = serviceNowAccount.ServiceSecret;

            //submit incident to ServiceNow API
            AddNewIncidentResponse result = await _serviceNowService.AddNewIncidentAsync(username, password, addNewIncidentRequest, cancellationToken);

            var incidentSubmittedResult = new IncidentSubmittedResultIntegrationEvent()
            {
                AlertId = result.Result.AlertId,
                ResponseCode = result.ResponseCode,
                Message = result.Result.Message
            };

            await _eventBusQueuePublisher.PublishAsync(incidentSubmittedResult);
            return Unit.Value;
        }
    }
}
