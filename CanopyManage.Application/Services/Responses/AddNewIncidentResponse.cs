using Newtonsoft.Json;

namespace CanopyManage.Application.Services.Responses
{
    public class AddNewIncidentResponse
    {
        [JsonProperty("result")]
        public IncidentResult Result {get;set; }

        [JsonIgnore()]
        public string ResponseCode { get; set; }
    }

    public class IncidentResult {

        [JsonProperty("sys_id")]
        public string AlertId { get; set; }

        [JsonProperty("short_description")]
        public string Message { get; set; }
    }
}
