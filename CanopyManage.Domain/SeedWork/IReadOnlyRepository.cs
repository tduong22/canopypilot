using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace CanopyManage.Domain.SeedWork
{
    public interface IReadOnlyRepository<TEntity, TIdentity> 
        where TEntity : BaseEntity<TIdentity>
    {
        Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken);
        Task<TEntity> GetByIdAsync(TIdentity id, CancellationToken cancellationToken);
        Task<IQueryable<TEntity>> FindByAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken);
    }
}
