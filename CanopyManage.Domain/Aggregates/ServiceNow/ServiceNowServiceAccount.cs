using CanopyManage.Domain.ValueObjects;

namespace CanopyManage.Domain.Aggregates
{
    public class ServiceNowServiceAccount : ServiceAccount
    {
        public ServiceNowServiceAccount(int serviceNowSettingId)
        {
            ServiceNowSettingId = serviceNowSettingId;
        }

        public int ServiceNowSettingId { get; set; }
        public ServiceNowSetting ServiceNowSetting { get; set; }
    }
}
