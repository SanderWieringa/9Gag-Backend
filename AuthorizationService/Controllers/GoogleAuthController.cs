using Google.Apis.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthorizationService.Controllers
{
    public class GoogleAuthController : ControllerBase
    {
        public GoogleAuthController()
        {

        }

        [HttpPost]
        [AllowAnonymous]
        [Route("test/{token}")]
        public async Task<IActionResult> Test([FromRoute] string token)
        {
            try
            {
                var googleUser = await GoogleJsonWebSignature.ValidateAsync(token, new GoogleJsonWebSignature.ValidationSettings()
                {
                    Audience = new[] { "86108609563-g3elr6e4kbqiqv677nuu1kltsul1sb0j.apps.googleusercontent.com" }
                });

                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
