using CanopyManage.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CanopyManage.Application.Compositions
{
    public static class ExternalServiceComposition
    {
        public static IServiceCollection AddExternalServices(this IServiceCollection services)
        {
            services.AddHttpClient<IServiceNowService, ServiceNowService>();

            return services;
        }
    }
}
