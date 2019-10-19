using CanopyManage.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace CanopyManage.Domain.ValueObjects
{
    public class ExternalServiceInfo : ValueObject
    {
        public string ServiceName { get; set; }
        public string Environment { get; set; }
        public string Owner { get; set; }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return ServiceName;
            yield return Environment;
            yield return Owner;
        }
    }
}
