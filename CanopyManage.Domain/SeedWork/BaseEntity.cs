using MediatR;
using System;
using System.Collections.Generic;

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
        private List<INotification> _domainEvents;
        public IReadOnlyCollection<INotification> DomainEvents => _domainEvents?.AsReadOnly();

        public void AddDomainEvent(INotification eventItem)
        {
            _domainEvents = _domainEvents ?? new List<INotification>();
            _domainEvents.Add(eventItem);
        }

        public void RemoveDomainEvent(INotification eventItem)
        {
            _domainEvents?.Remove(eventItem);
        }

        public void ClearDomainEvents()
        {
            _domainEvents?.Clear();
        }
    }
}
