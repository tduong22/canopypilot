using System.Threading.Tasks;

namespace CanopyManage.Infrastructure.KeyVault
{
    public interface IConfigKeyVault
    {
        Task<string> GetSecureSecret(string secretName);
    }
}
