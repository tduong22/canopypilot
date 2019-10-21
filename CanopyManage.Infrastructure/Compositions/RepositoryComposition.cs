using CanopyManage.Domain.Aggregates;
using CanopyManage.Domain.SeedWork;
using CanopyManage.Infrastructure.Azure.KeyVault;
using CanopyManage.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace CanopyManage.Infrastructure.Compositions
{
    public static class RepositoryComposition
    {
        public static IServiceCollection AddRepository(this IServiceCollection services)
        {
            //inject client
            services.AddScoped<IKeyVaultAuthenticator, KeyVaultAuthenticator>();
            services.AddScoped<IDataKeyVault<string, ServiceNowServiceAccount>>((sp) =>
            {
                var clientId = "4188b8e1-b747-4db3-af27-b4ac179260a3";
                var clientSecret = "@B_Ft/zQbr4.w9ekCEm1CAa2dQfU1u?T";
                var keyVaultEndpoint = "https://canopy-test-vlt.vault.azure.net/";
                var authenticator = sp.GetRequiredService<IKeyVaultAuthenticator>();
                return new ServiceNowServiceAccountDataKeyVault(authenticator, clientSecret, clientId, keyVaultEndpoint);
            });
            services.AddScoped<IRepository<ServiceNowServiceAccount, string>, ServiceAccountKeyVaultRepository>();

            return services;
        }
    }
}
