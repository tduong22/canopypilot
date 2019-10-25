using Newtonsoft.Json;

namespace CanopyManage.Application.Services.Requests
{
    public class AddNewIncidentRequest
    {
        public string AlertId {get; set; }

        [JsonProperty("short_description")]
        public string Title { get; set; }

        [JsonProperty("description")]
        public string Message { get; set; }
    }
}
