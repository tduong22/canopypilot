using Newtonsoft.Json;
using System;

namespace CanopyManage.Common.EventBus.Events
{
    public class IntegrationEvent : Event
    {
        public IntegrationEvent() : base()
        {
        }

        [JsonConstructor]
        public IntegrationEvent(Guid id, DateTime createDate) : base(id, createDate)
        {
        }

        [JsonConstructor]
        public IntegrationEvent(Guid id, Guid correlationId, DateTime createDate) : base(id, correlationId, createDate)
        {
        }

    }
}
