using CanopyManage.Application.Commands.SubmitServiceAccount;
using CanopyManage.WebService.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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

        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ServiceAccountSubmissionRequest request)
        {
            try
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
            catch (Exception ex)
            {

                return BadRequest();
            }
        }
    }
}