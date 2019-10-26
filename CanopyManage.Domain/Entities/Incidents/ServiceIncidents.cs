using CanopyManage.Domain.Entities.Services;
using CanopyManage.Domain.SeedWork;
using System;

namespace CanopyManage.Domain.Entities
{
    public enum ServiceIncidentSeverity
    {
        Information = 10,
        Warning = 20,
        Serious = 30,
        Apocalypse = 9999
    }

    public enum ServiceIncidentStatus
    {
        Initial = 10,
        Submitted = 20,
        Rejected = 30,
        Unverified = 9999
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
        public ServiceIncidentStatus Status {get;set; }

        public ServiceIncidentSeverity ServiceIncidentSeverity { get; set; }

        public ServiceIncident()
        {
        }
    }
}
