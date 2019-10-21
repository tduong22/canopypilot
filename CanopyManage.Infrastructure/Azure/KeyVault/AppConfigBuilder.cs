using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureKeyVault;

namespace CanopyManage.Infrastructure.Azure.KeyVault
{
    public class AppConfigBuilder
    {
        public static void ConfigAzureKevaultConfiguration(WebHostBuilderContext context, IConfigurationBuilder config)
        {
            var builtConfig = config.Build();
            var azureServiceTokenProvider = new AzureServiceTokenProvider();
            var keyVaultClient = new KeyVaultClient(
                new KeyVaultClient.AuthenticationCallback(
                    azureServiceTokenProvider.KeyVaultTokenCallback));
            
            config.AddAzureKeyVault(
                $"https://{builtConfig["KeyVaultName"]}.vault.azure.net/",
                keyVaultClient,
                new DefaultKeyVaultSecretManager());
        }
    }
}
