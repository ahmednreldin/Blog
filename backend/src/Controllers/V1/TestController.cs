using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenSchool.src.Contract.V1;

namespace OpenSchool.src.Controllers
{
    [ApiController]
    public class TestController : ControllerBase
    {

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet(ApiRoutes.Tests.tst401)]
        public IActionResult TestAuthentication()
        {
            return Ok("You're Authenticated");
        }

        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
        [HttpGet(ApiRoutes.Tests.tst403)]
        public IActionResult TestAuthorization()
        {
            return Ok("You're Authorized");
        }

    }
}
