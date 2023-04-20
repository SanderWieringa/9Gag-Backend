using Google.Apis.Auth;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.Cookies;
using AuthorizationService.Data;

namespace AuthorizationService.Controllers
{
    public class GoogleAuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public GoogleAuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [Route("signup")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Register()
        {
            if (!ModelState.IsValid) return BadRequest();
            var properties = new AuthenticationProperties { RedirectUri = Url.Action("GoogleResponse") };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        [Route("google-response")]
        public async Task<IActionResult> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            var jwt = await _authService.AuthorizeAsync(result);

            return Ok(jwt);
        }

        /*[HttpPost]
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
        }*/
    }
}
