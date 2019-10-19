using System;
using System.Collections.Generic;
using System.Text;

namespace CanopyManage.Domain.SeedWork
{
    public interface IReadOnlyRepository<TEntity, TIdentity> where TEntity : BaseEntity<TIdentity>
    {
        IEnumerable<TEntity> GetAll();
        TEntity Get(TIdentity id);
    }
}
