using CanopyManage.Application.Commands.SubmitAlert;
using CanopyManage.IncidentService.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace CanopyManage.IncidentService.Controllers
{
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
                AlertId = request.AlertId,
                ServiceNowSettingID = request.ServiceNowSettingID,
                AlertType = request.AlertType,
                Title = request.Title,
                Message = request.Message
            };

            await mediator.Send(command);

            return Ok();
        }
    }
}