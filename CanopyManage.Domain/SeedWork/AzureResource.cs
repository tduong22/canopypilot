using Ardalis.GuardClauses;
using System;

namespace CanopyManage.Domain.SeedWork
{
    public class AzureResource<TEntity> : BaseEntity<string> where TEntity : class
    {
        public AzureResource(string tenantId, TEntity entity)
        {
            Guard.Against.NullOrEmpty(tenantId, nameof(tenantId));
            TenantId = tenantId;

            Guard.Against.Null(entity, nameof(entity));
            Entity = entity;
        }

        public string TenantId { get; set; }
        public TEntity Entity { get; set; }
    }
}
