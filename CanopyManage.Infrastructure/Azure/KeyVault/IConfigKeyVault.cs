using System.Threading;
using System.Threading.Tasks;

namespace CanopyManage.Infrastructure.KeyVault
{
    public interface IConfigKeyVault
    {
        Task<string> GetSecureSecretAsync(string secretName, CancellationToken cancellationToken);
    }
}
