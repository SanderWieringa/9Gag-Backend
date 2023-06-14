using AuthorizationService.Models;
using Microsoft.AspNetCore.Authentication;
using static Google.Apis.Auth.GoogleJsonWebSignature;

namespace AuthorizationService.Data
{
    public interface IAuthService
    {
        Task<User> Authenticate(Payload payload);
        /*User GetUserByEmail(Payload payload);*/
        void RemoveUser(string jwt);

        /*Task<string> AuthorizeAsync(AuthenticateResult authResult);
        Task<bool> CheckAccountExistsAsync(string email);*/
    }
}
