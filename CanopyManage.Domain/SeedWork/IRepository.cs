using System.Collections.Generic;

namespace CanopyManage.Domain.SeedWork
{
    public interface IRepository<TEntity, TIdentity> where TEntity : BaseEntity<TIdentity>
    {
        IEnumerable<TEntity> GetAll();
        TEntity Get(TIdentity id);
        void Insert(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
    }
}
