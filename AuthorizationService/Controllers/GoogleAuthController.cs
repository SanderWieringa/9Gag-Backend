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
                    Audience = new[] { "529825921685-048hgv1dpce1uadc118pgfcak28u8n3a.apps.googleusercontent.com" }
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
