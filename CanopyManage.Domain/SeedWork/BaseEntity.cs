using System;

namespace CanopyManage.Domain.SeedWork
{
    public abstract class BaseEntity<TIdentity> : IEntity
    {
        private TIdentity id = default(TIdentity);

        public virtual TIdentity Id
        {
            get
            {
                return id;
            }
            protected set
            {
                if (!id.Equals(default(TIdentity)))
                {
                    throw new InvalidOperationException("Entity ID cannot be changed");
                }

                id = value;
            }
        }
    }
}
