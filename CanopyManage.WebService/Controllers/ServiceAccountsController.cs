using CanopyManage.Application.Commands.SubmitServiceAccount;
using CanopyManage.WebService.Requests;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace CanopyManage.WebService.Controllers
{

    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class ServiceAccountsController : ControllerBase
    {
        private readonly IMediator mediator;

        public ServiceAccountsController(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ServiceAccountSubmissionRequest request)
        {
            var command = new SubmitServiceAccountCommand()
            {
                ServiceNowSettingID = request.ServiceNowSettingID,
                TenantId = request.TenantId,
                ServiceNowUsername = request.ServiceNowUsername,
                ServiceNowPassword = request.ServiceNowPassword
            };

            await mediator.Send(command);

            return Ok();
        }
    }
}