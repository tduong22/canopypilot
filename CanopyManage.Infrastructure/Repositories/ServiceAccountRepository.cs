using CanopyManage.Domain.Aggregates;
using CanopyManage.Domain.SeedWork;
using CanopyManage.Infrastructure.Azure.KeyVault;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace CanopyManage.Infrastructure.Repositories
{
    public class ServiceAccountKeyVaultRepository : IRepository<ServiceNowServiceAccount, string>
    {
        private readonly IDataKeyVault _dataKeyVault;

        public ServiceAccountKeyVaultRepository(IDataKeyVault dataKeyVault)
        {
            _dataKeyVault = dataKeyVault;
        }

        public Task DeleteAsync(ServiceNowServiceAccount entity, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IQueryable<ServiceNowServiceAccount>> FindByAsync(Expression<Func<ServiceNowServiceAccount, bool>> predicate, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ServiceNowServiceAccount>> GetAllAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceNowServiceAccount> GetByIdAsync(string id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceNowServiceAccount> InsertAsync(ServiceNowServiceAccount entity, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceNowServiceAccount> UpdateAsync(ServiceNowServiceAccount entity, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }

}
