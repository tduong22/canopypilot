using Newtonsoft.Json;

namespace CanopyManage.Application.Services.Responses
{
    public class AddNewIncidentResponse
    {
        [JsonProperty("short_description")]
        public string Title { get; set; }

        [JsonProperty("description")]
        public string Message { get; set; }
    }
}
