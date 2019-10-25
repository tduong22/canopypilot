using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace CanopyManage.Common.EventBusServiceBus.Options
{
    [DataContract]
    public class ServiceBusOption
    {
        /// <summary>
        /// The name of the subscription. If not existed, the client will automatically create one with the name.
        /// </summary>
        [DataMember]
        public string SubscriptionName { get; set; }
        /// <summary>
        /// If subscription require sessions. If specified true, also update SessionIdFilter so that the subscription can filter out un-matched messages. 
        /// </summary>
        [DataMember]
        public bool SubscriptionRequireSession { get; set; } = true;

        /// <summary>
        /// If subscription require sessions. If specified true, also update SessionIdFilter so that the subscription can filter out un-matched messages. 
        /// </summary>
        [DataMember]
        public bool SubscriptionRequirePartition { get; set; } = true;

        /// <summary>
        /// Require to check whether the subscription is existed before, otherwise created if not existed. Default is true.
        /// </summary>
        [DataMember]
        public bool CheckSubscriptionExist { get; set; } = true;
        /// <summary>
        /// Specify if there is need of decompress the message on the receiver side.
        /// </summary>
        [DataMember]
        public bool IsCompressed { get; set; } = false;

        [IgnoreDataMember]
        public Encoding Encoding = Encoding.UTF8;

        /// <summary>
        /// Maximum concurrent connections (sessions)
        /// </summary>
        [DataMember]
        public int MaxConcurrentSessions { get; set; } = 1;

        /// <summary>
        /// Along with SubscriptionRequireSession, specify the key that used to filter the messages on the receiver side
        /// </summary>
        [DataMember]
        public string SessionIdFilter { get; set; }

        /// <summary>
        /// Along with SubscriptionRequireSession, specify the key that used to filter the messages on the receiver side
        /// </summary>
        [DataMember]
        public string PartitionKeyFilter { get; set; }

        /// <summary>
        /// The Service Bus Connection string with topic name in it.
        /// </summary>
        [DataMember]
        public string ConnectionString { get; set; }

        /// <summary>
        /// Auto-complete without manually complete the message. Default is false.
        /// </summary>
        [DataMember]
        public bool AutoCompleted { get; set; } = false;

        /// <summary>
        /// Default is Integration Event
        /// </summary>
        [DataMember]
        public EventType EventType { get; set; } = EventType.IntegrationEvent;

        /// <summary>
        /// Default is Integration Event
        /// </summary>
        [DataMember]
        public int PrefetchCount { get; set; }


        /// <summary>
        /// For custom data filters.
        /// </summary>
        [DataMember]
        public Dictionary<string, string> CustomFilters { get; set; }
    }

    [DataContract]
    public enum ClientMode
    {
        [EnumMember]
        Sending = 0,
        [EnumMember]
        Receiving = 1,
        [EnumMember]
        Both = 2
    }

    [DataContract]
    public enum EventType
    {
        [EnumMember]
        IntegrationEvent = 0,
        [EnumMember]
        DomainEvent = 1

    }
}
