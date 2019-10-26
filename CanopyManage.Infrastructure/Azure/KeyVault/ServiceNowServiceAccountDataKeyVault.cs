using CanopyManage.Domain.Entities;
using Microsoft.Azure.KeyVault;
using System.Threading;
using System.Threading.Tasks;

namespace CanopyManage.Infrastructure.Azure.KeyVault
{
    public class ServiceNowServiceAccountDataKeyVault : IDataKeyVault<string, ServiceNowServiceAccount>
    {
        private string _clientSecret;
        private string _clientId;
        private string _keyVaultEndPoint;

        private const string _userNamePostfix = "UserName";
        private const string _passwordPostfix = "Password";

        private readonly IKeyVaultAuthenticator _keyVaultAuthenticator;
        private readonly KeyVaultClient _keyVaultClient;
        public ServiceNowServiceAccountDataKeyVault(IKeyVaultAuthenticator keyVaultAuthenticator,
                                                    string clientSecret,
                                                    string clientId,
                                                    string keyVaultEndPoint)
        {
            _keyVaultAuthenticator = keyVaultAuthenticator;
            _keyVaultClient = new KeyVaultClient(GetTokenAsync);
            _clientSecret = clientSecret;
            _clientId = clientId;
            _keyVaultEndPoint = keyVaultEndPoint;
        }

        public Task<string> GetTokenAsync(string authority, string resource, string scope)
        {
            return _keyVaultAuthenticator.GetToken(_clientId, _clientSecret, authority, resource, scope);
        }

        public async Task<ServiceNowServiceAccount> CreateSecretAsync(string key, ServiceNowServiceAccount value, CancellationToken cancellationToken)
        {
            await _keyVaultClient.SetSecretAsync(_keyVaultEndPoint, $"{key}{_userNamePostfix}", value.ServiceUserName);
            await _keyVaultClient.SetSecretAsync(_keyVaultEndPoint, $"{key}{_passwordPostfix}", value.ServiceSecret);
            return value;
        }

        public async Task<ServiceNowServiceAccount> GetSecretAsync(string id, CancellationToken cancellationToken)
        {
            var userName = await _keyVaultClient.GetSecretAsync($"{id}{_userNamePostfix}", cancellationToken);
            var password = await _keyVaultClient.GetSecretAsync($"{id}{_passwordPostfix}", cancellationToken);

            return new ServiceNowServiceAccount(ServiceNowServiceAccount.GetServiceNowSettingIdFromId(id),
                                                ServiceNowServiceAccount.GetTenantIdFromId(id),
                                                userName?.Value,
                                                password?.Value);
        }
    }
}
