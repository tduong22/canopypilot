using System.Threading;
using System.Threading.Tasks;

namespace CanopyManage.Infrastructure.Azure.KeyVault
{
    public interface IDataKeyVault<TKey, TValue>
    {
        Task<TValue> GetSecretAsync(TKey key, CancellationToken cancellationToken);
        Task<TValue> CreateSecretAsync(TKey key, TValue value, CancellationToken cancellationToken);
    }
}
