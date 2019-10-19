using Microsoft.Azure.KeyVault;
using System.Threading;
using System.Threading.Tasks;

namespace CanopyManage.Infrastructure.Azure.KeyVault
{
    public class DataKeyVault : IDataKeyVault
    {
        private string _clientSecret;
        private string _clientId;
        private string _keyVaultEndPoint;

        private readonly IKeyVaultAuthenticator _keyVaultAuthenticator;
        private readonly KeyVaultClient _keyVaultClient;
        public DataKeyVault(IKeyVaultAuthenticator keyVaultAuthenticator, KeyVaultClient keyVaultClient, string _clientSecret, string _clientId, string _keyVaultEndPoint)
        {
            _keyVaultAuthenticator = keyVaultAuthenticator;
            _keyVaultClient = keyVaultClient;
        }

        public Task<string> GetTokenAsync(string authority, string resource, string scope)
        {
            return _keyVaultAuthenticator.GetToken(_clientId, _clientSecret, authority, resource, scope);
        }

        public Task CreateSecretAsync(string id, string value, CancellationToken cancellationToken)
        {
            return _keyVaultClient.SetSecretAsync(_keyVaultEndPoint, id, value);
        }

        public async Task<string> GetSecretAsync(string id, CancellationToken cancellationToken)
        {
            var secret = await _keyVaultClient.GetSecretAsync(id, cancellationToken);
            return secret.Value;
        }
    }
}
