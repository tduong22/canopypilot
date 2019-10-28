using CanopyManage.Application.Commands.SubmitAlert;
using CanopyManage.IncidentService.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace CanopyManage.IncidentService.Controllers
{
    /// <summary>
    /// Incidents Controller serves for submitting alert messages to the service bus topic
    /// Used for testing purpose or if HTTP is the only protocol supported
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class IncidentsController : ControllerBase
    {
        private readonly IMediator mediator;

        public IncidentsController(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] AlertSubmissionRequest request)
        {
            var command = new SubmitAlertCommand()
            {
                TenantId = request.TenantId,
                AlertId = request.AlertId,
                ServiceNowSettingID = request.ServiceNowSettingID,
                AlertType = request.AlertType,
                Title = request.Title,
                Message = request.Message
            };

            await mediator.Send(command);

            return CreatedAtAction(nameof(Post), request);
        }
    }
}