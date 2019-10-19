﻿using CanopyManage.Common.EventBus.Events;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CanopyManage.Common.EventBus.Abstractions
{
    public interface IEventBusPublisher
    {
        Task PublishAsync(Event @event, string partitionKey, IDictionary<string, string> userDictionary = null);

        Task PublishAsync(IEnumerable<Event> eventList, string partitionKey, IDictionary<string, string> userDictionary = null);
    }
}
