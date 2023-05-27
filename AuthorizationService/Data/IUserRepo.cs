using AuthorizationService.Models;

namespace AuthorizationService.Data
{
    public interface IUserRepo
    {
        IEnumerable<User> GetAllUsers();
        void CreateUser(User user);
        /*bool SaveChanges();*/
    }
}
