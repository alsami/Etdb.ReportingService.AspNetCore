using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Etdb.ReportingService.Controllers.V1
{
    [ApiController]
    [Route("api/v1/dummy")]
    public class DummyController : ControllerBase
    {
        [HttpGet]
        public IActionResult AuthTest()
        {
            return this.Ok(new
            {
                Message = $"{this.HttpContext.User.Identity.Name} You can see this since you were successfully authenticated"
            });
        }

        [HttpGet("noauth")]
        [AllowAnonymous]
        public IActionResult NoAuthTest()
        {
            return this.Ok(new
            {
                Message = "You can see this since because this endpoint does not required a user to be authenticated"
            });
        }
    }
}