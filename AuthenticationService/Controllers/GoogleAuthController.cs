using AuthenticationService.Requests;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace AuthenticationService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GoogleAuthController : ControllerBase
    {
        public GoogleAuthController()
        {
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
            return Ok(payload);
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
