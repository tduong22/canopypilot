using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CanopyManage.Infrastructure.Azure.KeyVault
{
    public interface IKeyVaultAuthenticator
    {
        Task<string> GetToken(string _clientId, string _clientSecret, string authority, string resource, string scope);
    }
}
