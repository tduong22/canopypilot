using CanopyManage.Domain.SeedWork;
using CanopyManage.Domain.ValueObjects;

namespace CanopyManage.Domain.Aggregates
{
    public class ServiceAccount : BaseEntity<string>
    {
        public override string Id
        {
            get => base.Id;
            protected set => base.Id = value;
        }

        public ExternalServiceInfo ExternalServiceInfo { get; set; }
        public string ServiceUserName { get; set; }
        public string ServiceSecret { get; set; }

        public ServiceAccount()
        {

        }
    }
}
