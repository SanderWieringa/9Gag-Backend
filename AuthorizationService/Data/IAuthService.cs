using Microsoft.AspNetCore.Authentication;

namespace AuthorizationService.Data
{
    public interface IAuthService
    {
        Task<string> AuthorizeAsync(AuthenticateResult authResult);
        Task<bool> CheckAccountExistsAsync(string email);
    }
}
