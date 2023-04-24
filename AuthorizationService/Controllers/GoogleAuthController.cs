using Google.Apis.Auth;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.Cookies;
using AuthorizationService.Data;
using AuthorizationService.Requests;

namespace AuthorizationService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GoogleAuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public GoogleAuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        [Route("signup")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Verify([FromBody] RegisterRequest request)
        {
            Console.WriteLine("request: ", request.Credential.ToString());
            string token = request.Credential;
            var payload = await VerifyGoogleTokenId(token);
            if (payload == null)
            {
                return BadRequest("Invalid token");
            }
            //return Challenge(payload.ToString(), GoogleDefaults.AuthenticationScheme);
            return Ok(payload);
        }

        private async Task<GoogleJsonWebSignature.Payload> VerifyGoogleTokenId(string token)
        {
            try
            {
                // uncomment these lines if you want to add settings: 
                // var validationSettings = new GoogleJsonWebSignature.ValidationSettings
                // { 
                //     Audience = new string[] { "yourServerClientIdFromGoogleConsole.apps.googleusercontent.com" }
                // };
                // Add your settings and then get the payload
                // GoogleJsonWebSignature.Payload payload =  await GoogleJsonWebSignature.ValidateAsync(token, validationSettings);

                // Or Get the payload without settings.
                GoogleJsonWebSignature.Payload payload = await GoogleJsonWebSignature.ValidateAsync(token);

                return payload;
            }
            catch (System.Exception)
            {
                Console.WriteLine("invalid google token");

            }
            return null;
        }

        /*[HttpPost]
        [Route("signup")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Register()
        {
        *//*
        1. VerifyGoogleToken
        2. push DB
        3. sign jwt
         */

        /*if (!ModelState.IsValid) return BadRequest();
        var properties = new AuthenticationProperties { RedirectUri = Url.Action("GoogleResponse") };
        return Challenge(properties, GoogleDefaults.AuthenticationScheme);*//*
        return default;
    }*/

        [HttpPost]
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
