using CanopyManage.Domain.Aggregates;
using CanopyManage.Domain.SeedWork;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CanopyManage.Application.Commands.SubmitServiceAccount
{
    public class SubmitServiceAccountCommandHandler : IRequestHandler<SubmitServiceAccountCommand>
    {
        private readonly IRepository<AzureResource<ServiceNowServiceAccount>, string> repository;

        public SubmitServiceAccountCommandHandler(IRepository<AzureResource<ServiceNowServiceAccount>, string> repository)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<Unit> Handle(SubmitServiceAccountCommand request, CancellationToken cancellationToken)
        {
            var serviceNowServiceAccount = new ServiceNowServiceAccount(request.ServiceNowSettingID);
            var azureResource = new AzureResource<ServiceNowServiceAccount>(request.TenantId, serviceNowServiceAccount);

            await repository.InsertAsync(azureResource, cancellationToken);

            return Unit.Value;
        }
    }
}
