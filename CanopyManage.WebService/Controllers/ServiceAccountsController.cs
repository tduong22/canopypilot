using Microsoft.AspNetCore.Mvc;

namespace CanopyManage.WebService.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class ServiceAccountsController : ControllerBase
    {
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }
    }
}