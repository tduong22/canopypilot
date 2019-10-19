using System.Threading;
using System.Threading.Tasks;

namespace CanopyManage.Infrastructure.Azure.KeyVault
{
    public interface IDataKeyVault
    {
        Task<string> GetSecretAsync(string key, CancellationToken cancellationToken);
        Task CreateSecretAsync(string key, string value, CancellationToken cancellationToken);
    }
}
