using CanopyManage.Domain.SeedWork;
using CanopyManage.Domain.ValueObjects;

namespace CanopyManage.Domain.Aggregates
{
    public class ServiceNowServiceAccount : ServiceAccount, IAzureResource
    {
        private string _tenantId;
        public ServiceNowSetting ServiceNowSetting { get; set; }

        public string TenantId { get => _tenantId; set => _tenantId = value; }
    }
}
