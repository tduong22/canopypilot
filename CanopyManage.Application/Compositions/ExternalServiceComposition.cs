using CanopyManage.Application.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CanopyManage.Application.Compositions
{
    public static class ExternalServiceComposition
    {
        public static IServiceCollection AddExternalServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient<IServiceNowService, ServiceNowService>("servicenow", client => { 
                   client.BaseAddress = new System.Uri(configuration["ServiceNow:BaseUrl"]);
            });
            return services;
        }
    }
}
