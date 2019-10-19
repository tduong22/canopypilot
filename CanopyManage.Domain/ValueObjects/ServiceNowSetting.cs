using System.Collections.Generic;
using CanopyManage.Domain.SeedWork;

namespace CanopyManage.Domain.ValueObjects
{
    public class ServiceNowSetting : ValueObject
    {
        public ServiceNowSetting() {

        }
        public string ServiceNowSettingId { get; set; }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return ServiceNowSettingId;
        }
    }
}
