// registration , request token 
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using OpenSchool.src.Contract.V1;
using OpenSchool.src.Models;
using OpenSchool.src.Services;
namespace OpenSchool.src.Controllers.V1
{

    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost(ApiRoutes.Auths.signUp)]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterModel model)
        {
            // Check Model-Binding Validation 
            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            // check Registration 
            var result = await _authService.RegisterAsync(model);

            // if registration fail
            if (!result.IsAuthenticated)
                 return Conflict(result);

            return Ok(result);
        }

        [HttpPost(ApiRoutes.Auths.signIn)]
        public async Task<IActionResult> GetTokenAsync([FromBody] TokenRequestModel model)
        {
            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }
            var result = await _authService.GetTokenAsync(model);

            if (result.IncompleteForm)
            {
                return UnprocessableEntity(result);
            }
            
            if (!result.IsAuthenticated)
                return NotFound(result);
                
            return Ok(result);
        }

    }
}
