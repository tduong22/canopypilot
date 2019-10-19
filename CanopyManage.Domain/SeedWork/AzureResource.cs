using System;
using System.Collections.Generic;
using System.Text;

namespace CanopyManage.Domain.SeedWork
{
    public class AzureResource<TEntity> : BaseEntity<string>
    {
        public string TenantId { get; set; }
    }
}
