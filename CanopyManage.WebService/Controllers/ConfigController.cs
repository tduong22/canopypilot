using CanopyManage.Application.Commands.SubmitServiceAccount;
using CanopyManage.WebService.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CanopyManage.WebService.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class ConfigController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public ConfigController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public ActionResult<string> Get()
        {
            return _configuration["TesConfig:TestValue"] + _configuration["KeyVaultName"];
        }
    }
}