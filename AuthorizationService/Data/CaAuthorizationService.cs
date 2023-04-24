using AuthorizationService.Dtos;
using AuthorizationService.Models;
using Microsoft.AspNetCore.Authentication;

namespace AuthorizationService.Data
{
    public class CaAuthorizationService : IAuthService
    {
        private readonly IAuthorizationContext authorizationContext;

        public async Task<string> AuthorizeAsync(AuthenticateResult authResult)
        {
            //  AuthResponseDto response = await _authHttpRequest.SendAuthRequest(code);
            //List<Claim> claims = authResult.Principal.Claims.ToList();

            var claims = authResult.Principal.Identities.FirstOrDefault()
                .Claims.Select(claim => new
                {
                    claim.Issuer,
                    claim.OriginalIssuer,
                    claim.Type,
                    claim.Value,
                    claim.Subject
                });
            var email = claims.FirstOrDefault(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress").Value;
            var name = claims.FirstOrDefault(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name").Value;
            // var identifier = claims.FirstOrDefault(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/identifier").Value;
            //Check if Account already exists, and register one if this is not the case. Afterwards generate a JWT
            if (!CheckAccountExistsAsync(email).Result)
            {
                await CreateAccountAsync(email, name);
            }

            return null;//TokenGenerator.GenerateToken(JWTSettings.SecretKey, email);
        }

        public async Task<bool> CheckAccountExistsAsync(string email)
        {
            return authorizationContext.Users.FirstOrDefault(x => x.Email == email) != null;
        }

        private async Task<bool> CreateAccountAsync(string email, string name)
        {
            User newUser = new User()
            {
                Id = Guid.NewGuid(),
                Name = name,
                Email = email
            };
            authorizationContext.Users.Add(newUser);
            bool success = await authorizationContext.SaveChangesAsync() > 0;

            UserDto userDTO = newUser;

            //await _publisher.PublishMessageAsync<UserDTO>("UserCreatedEvent", userDTO);
            if (!success)
            {
                throw new InvalidOperationException("Something went wrong trying to create the account");
            }
            return true;
        }
    }
}
