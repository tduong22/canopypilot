using CanopyManage.Domain.Entities;
using CanopyManage.Domain.SeedWork;
using CanopyManage.Infrastructure.Azure.KeyVault;
using CanopyManage.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace CanopyManage.Infrastructure.Compositions
{
    public static class RepositoryComposition
    {
        public static IServiceCollection AddKeyVaultRepository(this IServiceCollection services, string applicationClientId, string applicationClientSecret, string keyVaultUri)
        {
            services.AddScoped<IKeyVaultAuthenticator, KeyVaultAuthenticator>();
            services.AddScoped<IDataKeyVault<string, ServiceNowServiceAccount>>((sp) =>
            {
                var clientId = applicationClientId;
                var clientSecret = applicationClientSecret;
                var keyVaultEndpoint = keyVaultUri;
                var authenticator = sp.GetRequiredService<IKeyVaultAuthenticator>();
                return new ServiceNowServiceAccountDataKeyVault(authenticator, clientSecret, clientId, keyVaultEndpoint);
            });
            services.AddScoped<IRepository<ServiceNowServiceAccount, string>, ServiceAccountKeyVaultRepository>();

            return services;
        }
    }
}
