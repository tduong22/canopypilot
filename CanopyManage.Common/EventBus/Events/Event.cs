using Newtonsoft.Json;
using System;

namespace CanopyManage.Common.EventBus.Events
{
    public class Event
    {
        public Event()
        {
            Id = Guid.NewGuid();
            CreationDate = DateTime.UtcNow;
        }
        public Event(Guid id, DateTime createDate)
        {
            Id = id;
            CreationDate = createDate;
        }

        public Event(Guid id, Guid correlationId, DateTime createDate)
        {
            Id = id;
            CreationDate = createDate;
            CorrelationId = correlationId;
        }

        [JsonProperty]
        public Guid CorrelationId { get; private set; }

        [JsonProperty]
        public Guid Id { get; private set; }

        [JsonProperty]
        public DateTime CreationDate { get; private set; }

        [JsonProperty]
        public string DynamicEventName { get; protected set; }

    }
}