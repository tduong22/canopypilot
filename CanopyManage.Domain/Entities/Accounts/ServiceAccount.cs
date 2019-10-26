using CanopyManage.Domain.Entities.Services;
using CanopyManage.Domain.SeedWork;

namespace CanopyManage.Domain.Entities
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
