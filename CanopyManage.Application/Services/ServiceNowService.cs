using CanopyManage.Application.Services.Requests;
using CanopyManage.Application.Services.Responses;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CanopyManage.Application.Services
{
    public class ServiceNowService : IServiceNowService
    {
        private readonly HttpClient _httpClient;

        public ServiceNowService(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<AddNewIncidentResponse> AddNewIncidentAsync(AddNewIncidentRequest request, CancellationToken cancellationToken = default)
        {
            string url = "https://dev87790.service-now.com/api/now/table/incident";

            var byteArray = Encoding.ASCII.GetBytes($"admin:Password1");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

            string bodyContent = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(bodyContent, Encoding.UTF8, "application/json");
            HttpResponseMessage response =
                await _httpClient.PostAsync(url, httpContent);

            if (!response.IsSuccessStatusCode)
            {
                
            }

            string responseContent = await response.Content.ReadAsStringAsync();
            AddNewIncidentResponse result = JsonConvert.DeserializeObject<AddNewIncidentResponse>(responseContent);

            return result;
        }
    }
}
