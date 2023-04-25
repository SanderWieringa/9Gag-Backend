using Google.Apis.Auth;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.Cookies;
using AuthorizationService.Data;
using AuthorizationService.Requests;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using AuthorizationService.Helpers;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace AuthorizationService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GoogleAuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IAuthService _authService;

        public GoogleAuthController(IAuthService authService, IConfiguration configuration)
        {
            _configuration = configuration;
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

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Route("google-response")]
        public async Task<IActionResult> GoogleResponse([FromBody] LoginRequest request)
        {
            var payload = await VerifyGoogleTokenId(request.Credential);
            var user = await _authService.Authenticate(payload);

            var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, Security.Encrypt(_configuration["JwtEmailEncryption"], user.email)),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["JwtSecret"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(String.Empty,
                  String.Empty,
                  claims,
                  expires: DateTime.Now.AddSeconds(55 * 60),
                  signingCredentials: creds);


            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token)
            });
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
