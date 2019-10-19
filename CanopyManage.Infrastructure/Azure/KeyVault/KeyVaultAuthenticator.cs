using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Threading.Tasks;

namespace CanopyManage.Infrastructure.Azure.KeyVault
{
    public class KeyVaultAuthenticator : IKeyVaultAuthenticator
    {
        public KeyVaultAuthenticator()
        {
        }

        public async Task<string> GetToken(string _clientId, string _clientSecret, string authority, string resource, string scope)
        {

            try
            {
                var authContext = new AuthenticationContext(authority);
                var clientCred = new ClientCredential(_clientId,
                    _clientSecret);
                var result = await authContext.AcquireTokenAsync(resource, clientCred).ConfigureAwait(false);

                if (result == null)
                    throw new InvalidOperationException("Failed to obtain the JWT token");

                return result.AccessToken;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
