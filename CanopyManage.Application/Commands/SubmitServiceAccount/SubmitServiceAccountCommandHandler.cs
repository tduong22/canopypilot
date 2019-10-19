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
        private readonly IRepository<ServiceNowServiceAccount, string> repository;

        public SubmitServiceAccountCommandHandler(IRepository<ServiceNowServiceAccount, string> repository)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<Unit> Handle(SubmitServiceAccountCommand request, CancellationToken cancellationToken)
        {
            var serviceNowServiceAccount = new ServiceNowServiceAccount(request.ServiceNowSettingID,
                                                                        request.TenantId,
                                                                        request.ServiceNowUsername,
                                                                        request.ServiceNowPassword);
            await repository.InsertAsync(serviceNowServiceAccount, cancellationToken);

            return Unit.Value;
        }
    }
}
