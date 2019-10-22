using CanopyManage.Domain.Aggregates;
using CanopyManage.Domain.SeedWork;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CanopyManage.Application.Commands.SubmitServiceAccount
{
    public class SubmitServiceAccountCommandHandler : IRequestHandler<SubmitServiceAccountCommand, SubmitServiceAccountCommandResponse>
    {
        private readonly IRepository<ServiceNowServiceAccount, string> repository;

        public SubmitServiceAccountCommandHandler(IRepository<ServiceNowServiceAccount, string> repository)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
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
            catch
            {
                result.IsSuccessful = false;
            }

            return result;
        }
    }
}
