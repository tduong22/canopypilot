using CanopyManage.Domain.SeedWork;
using CanopyManage.Domain.ValueObjects;
using System;

namespace CanopyManage.Domain.Aggregates
{
    public enum ServiceIncidentSeverity {
        Information = 10,
        Warning = 20,
        Serious = 30,
        Apocalypse = 9999
    }

    public class ServiceIncident : BaseEntity<string>
    {
        public override string Id
        {
            get => base.Id;
            protected set => base.Id = value;
        }

        public ExternalServiceInfo ExternalServiceInfo { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Message { get; set; }
        public DateTime DateTime { get; set; }
        public ServiceIncidentSeverity ServiceIncidentSeverity  { get; set; }
    }

    public class AlertIncident : ServiceIncident {

        public string AlertType { get; set; }
        public string AlertId { get; set; }
    }

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
