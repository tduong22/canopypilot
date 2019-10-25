using System.Threading;
using System.Threading.Tasks;

namespace CanopyManage.Domain.SeedWork
{
    public interface IRepository<TEntity, TIdentity> : IReadOnlyRepository<TEntity, TIdentity> 
        where TEntity : BaseEntity<TIdentity>
    {
        Task<TEntity> InsertAsync(TEntity entity, CancellationToken cancellationToken);
        Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken);
        Task DeleteAsync(TEntity entity, CancellationToken cancellationToken);
    }
}
