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
        private string _incidentUrl = "api/now/table/incident";

        public ServiceNowService(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<AddNewIncidentResponse> AddNewIncidentAsync(string userName, string password, AddNewIncidentRequest request, CancellationToken cancellationToken = default)
        { 
            var byteArray = Encoding.ASCII.GetBytes($"{userName}:{password}");
            string bodyContent = JsonConvert.SerializeObject(request);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post,_incidentUrl);
            httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
            httpRequestMessage.Content = new StringContent(bodyContent, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.SendAsync(httpRequestMessage);

            if (!response.IsSuccessStatusCode)
            {
                return new AddNewIncidentResponse()
                {
                    ResponseCode = ((int)response.StatusCode).ToString(),
                    Result = new IncidentResult()
                    {
                        Message = await response.Content.ReadAsStringAsync()
                    }
                };
            }

            string responseContent = await response.Content.ReadAsStringAsync();
            AddNewIncidentResponse result = JsonConvert.DeserializeObject<AddNewIncidentResponse>(responseContent);
            result.ResponseCode = ((int)response.StatusCode).ToString();
            return result;
        }
    }
}
