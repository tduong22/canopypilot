using CanopyManage.Domain.ValueObjects;

namespace CanopyManage.Domain.Aggregates
{
    public class ServiceNowAlertIncident : AlertIncident {

        private const string _serviceNowConst = "ServiceNow";
        public string ServiceNowSettingId { get; set; }

        public ServiceNowAlertIncident()
        {
            ExternalServiceInfo = new ExternalServiceInfo()
            {
                ServiceName = _serviceNowConst,
                Environment = "Default",
                Owner = string.Empty
            };
        }
    }
}
