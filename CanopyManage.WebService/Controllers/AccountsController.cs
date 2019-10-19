using Microsoft.AspNetCore.Mvc;

namespace CanopyManage.WebService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }
    }
}