using CanopyManage.Domain.Entities;
using CanopyManage.Domain.SeedWork;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CanopyManage.Application.Commands.SubmitServiceAccount
{
    public class SubmitServiceAccountCommandHandler : IRequestHandler<SubmitServiceAccountCommand, SubmitServiceAccountCommandResponse>
    {
        private readonly IRepository<ServiceNowServiceAccount, string> repository;
        private readonly ILogger<SubmitServiceAccountCommandHandler> _logger;

        public SubmitServiceAccountCommandHandler(IRepository<ServiceNowServiceAccount, string> repository, ILogger<SubmitServiceAccountCommandHandler> logger)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger;
        }

        public async Task<SubmitServiceAccountCommandResponse> Handle(SubmitServiceAccountCommand request, CancellationToken cancellationToken)
        {

            var result = new SubmitServiceAccountCommandResponse()
            {
                IsSuccessful = true,
                Username = request.ServiceNowUsername
            };

            try
            {
                var serviceNowServiceAccount = new ServiceNowServiceAccount(request.ServiceNowSettingID,
                                                                           request.TenantId,
                                                                           request.ServiceNowUsername,
                                                                           request.ServiceNowPassword);
                ServiceNowServiceAccount insertResult = await repository.InsertAsync(serviceNowServiceAccount, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(SubmitServiceAccountCommandHandler)} failed to store the account credentials. {ex.Message}");
                result.IsSuccessful = false;
            }

            return result;
        }
    }
}
