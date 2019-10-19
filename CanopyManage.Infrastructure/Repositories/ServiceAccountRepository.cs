using CanopyManage.Domain.Aggregates;
using CanopyManage.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace CanopyManage.Infrastructure.Repositories
{
    public class ServiceAccountKeyVaultRepository : IRepository<AzureResource<ServiceNowServiceAccount>, string>
    {
        public ServiceAccountKeyVaultRepository()
        {
        }

        public Task DeleteAsync(AzureResource<ServiceNowServiceAccount> entity, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IQueryable<AzureResource<ServiceNowServiceAccount>>> FindByAsync(Expression<Func<AzureResource<ServiceNowServiceAccount>, bool>> predicate, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<AzureResource<ServiceNowServiceAccount>>> GetAllAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<AzureResource<ServiceNowServiceAccount>> GetByIdAsync(string id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<AzureResource<ServiceNowServiceAccount>> InsertAsync(AzureResource<ServiceNowServiceAccount> entity, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<AzureResource<ServiceNowServiceAccount>> UpdateAsync(AzureResource<ServiceNowServiceAccount> entity, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }

}
