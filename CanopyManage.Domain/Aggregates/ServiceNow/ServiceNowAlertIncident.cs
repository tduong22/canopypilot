﻿using CanopyManage.Domain.SeedWork;
using CanopyManage.Domain.ValueObjects;

namespace CanopyManage.Domain.Aggregates
{
    public class ServiceNowAlertIncident : AlertIncident, IAzureResource
    {
        private const string _serviceNowConst = "ServiceNow";
        public ServiceNowSetting ServiceNowSetting { get; set; }
        public string TenantId { get; set;}

        public ServiceNowAlertIncident(int serviceNowSettingId, string tenantId, string title, string description, string message, string alertId, string alertType)
        {
            ServiceNowSetting.ServiceNowSettingId = serviceNowSettingId.ToString();
            TenantId = tenantId;
            Title = title;
            Description = description;
            Message = message;
            AlertId = alertId;
            AlertType = alertType;
            ExternalServiceInfo = new ExternalServiceInfo()
            {
                ServiceName = _serviceNowConst,
                Environment = "Default",
                Owner = string.Empty
            };
        }
    }
}
