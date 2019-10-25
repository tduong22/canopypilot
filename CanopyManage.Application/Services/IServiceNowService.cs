using CanopyManage.Application.Services.Requests;
using CanopyManage.Application.Services.Responses;
using System.Threading;
using System.Threading.Tasks;

namespace CanopyManage.Application.Services
{
    public interface IServiceNowService
    {
        Task<AddNewIncidentResponse> AddNewIncidentAsync(string userName, string password, AddNewIncidentRequest request, CancellationToken cancellationToken = default);

    }
}
