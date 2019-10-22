using CanopyManage.Application.Services.Requests;
using CanopyManage.Application.Services.Responses;
using System.Threading;
using System.Threading.Tasks;

namespace CanopyManage.Application.Services
{
    public interface IServiceNowService
    {
        Task<AddNewIncidentResponse> AddNewIncidentAsync(AddNewIncidentRequest request, CancellationToken cancellationToken = default);

    }
}
