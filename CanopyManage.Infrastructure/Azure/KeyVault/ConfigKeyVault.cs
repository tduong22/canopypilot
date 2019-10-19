using CanopyManage.Infrastructure.KeyVault;
using Microsoft.Azure.KeyVault;
using System.Threading;
using System.Threading.Tasks;

namespace CanopyManage.Infrastructure.Azure.KeyVault
{
    public class ConfigKeyVault : IConfigKeyVault
    {
        private string _clientSecret;
        private string _clientId;
        private string _keyVaultEndPoint;

        private readonly IKeyVaultAuthenticator _keyVaultAuthenticator;

        public ConfigKeyVault(string keyVaultEndpoint, string clientId, string clientSecret, IKeyVaultAuthenticator keyVaultAuthenticator)
        {
            _keyVaultAuthenticator = keyVaultAuthenticator;
            _keyVaultEndPoint = keyVaultEndpoint;
            _clientId = clientId;
            _clientSecret = clientSecret;
        }

        public Task<string> GetTokenAsync(string authority, string resource, string scope)
        {
            return _keyVaultAuthenticator.GetToken(_clientId, _clientSecret, authority, resource, scope);
        }
        public async Task<string> GetSecureSecretAsync(string secretName, CancellationToken cancellationToken)
        {
            var kv = new KeyVaultClient(GetTokenAsync);
            var sec = await kv.GetSecretAsync(_keyVaultEndPoint, secretName);
            return sec.Value;
        }
    }
}
