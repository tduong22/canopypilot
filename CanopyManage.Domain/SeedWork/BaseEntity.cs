using System;
using System.Collections.Generic;
using System.Text;

namespace CanopyManage.Domain.SeedWork
{
    public abstract class BaseEntity<TIdentity> : IEntity
    {
        private TIdentity id = default(TIdentity);

        public TIdentity Id
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
