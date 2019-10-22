using Newtonsoft.Json;

namespace CanopyManage.Application.Services.Requests
{
    public class AddNewIncidentRequest
    {
        [JsonProperty("short_description")]
        public string Title { get; set; }

        [JsonProperty("description")]
        public string Message { get; set; }
    }
}
