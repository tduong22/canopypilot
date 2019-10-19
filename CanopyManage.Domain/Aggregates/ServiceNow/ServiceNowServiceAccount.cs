using CanopyManage.Domain.ValueObjects;

namespace CanopyManage.Domain.Aggregates
{
    public class ServiceNowServiceAccount : ServiceAccount
    {
        public ServiceNowSetting ServiceNowSetting { get; set; }
    }
}
