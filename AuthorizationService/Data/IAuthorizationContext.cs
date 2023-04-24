using AuthorizationService.Models;

namespace AuthorizationService.Data
{
    public interface IAuthorizationContext
    {
        List<User> Users { get; set; }
        Task<int> SaveChangesAsync();
    }
}
