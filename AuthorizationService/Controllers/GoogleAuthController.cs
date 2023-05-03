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
                GoogleJsonWebSignature.Payload payload = await GoogleJsonWebSignature.ValidateAsync(token);

                return payload;
            }
            catch (Exception)
            {
                Console.WriteLine("invalid google token");

            }
            return null;
        }
    }
}
