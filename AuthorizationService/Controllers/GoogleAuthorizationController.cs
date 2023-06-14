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
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Http.HttpResults;

namespace AuthorizationService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GoogleAuthorizationController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IAuthService _authService;
        // test

        public GoogleAuthorizationController(IAuthService authService, IConfiguration configuration)
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
            if (payload != null)
            {
                await _authService.Authenticate(payload);

                return Ok(payload);
            }

            return BadRequest("Invalid token");
        }

        [HttpDelete]
        /*[Authorize]*/
        public IActionResult RemoveUser([FromBody] DeleteRequest request){
            _authService.RemoveUser(request.Jwt);
            return Ok();
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Route("google-response")]
        public async Task<IActionResult> GoogleResponse([FromBody] LoginRequest request)
        {
            var payload = await VerifyGoogleTokenId(request.Credential);
            var user = await _authService.Authenticate(payload);
            Console.WriteLine("payload.Audience: ", payload.Audience);
            Console.WriteLine("payload.Issuer: ", payload.Issuer);
            var jwtEmailEncryption = _configuration["JwtEmailEncryption"];
            var JwtSecret = _configuration["JwtSecret"];
            Console.WriteLine("_configuration[\"JwtEmailEncryption\"]: ", jwtEmailEncryption);
            Console.WriteLine("_configuration[\"JwtSecret\"]: ", JwtSecret);
            var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, Security.Encrypt(_configuration["JwtEmailEncryption"], user.email)),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Aud, (string)payload.Audience),
                    new ("userId", user.id.ToString())
                };

            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(/*"glyceric tiltyard setback resource wilding carport"*/_configuration["JwtSecret"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            /*var token = new JwtSecurityToken(String.Empty,
                  String.Empty,
                  claims,
                  expires: DateTime.Now.AddSeconds(28800),
                  signingCredentials: creds);*/

            var token = new JwtSecurityToken(
                issuer: "https://accounts.google.com",
                audience: "86108609563-g3elr6e4kbqiqv677nuu1kltsul1sb0j.apps.googleusercontent.com",
                claims,
                expires: DateTime.Now.AddSeconds(28800),
                signingCredentials: creds
            );

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token)
            });

            /*var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddSeconds(28800),
                Issuer = "https://accounts.google.com",
                Audience = "86108609563-g3elr6e4kbqiqv677nuu1kltsul1sb0j.apps.googleusercontent.com",
                SigningCredentials = creds
            };
            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            var jwt = tokenHandler.WriteToken(token);
            return Ok(jwt);*/
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
