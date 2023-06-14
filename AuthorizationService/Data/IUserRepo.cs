using AuthorizationService.Models;
using MongoDB.Bson;

namespace AuthorizationService.Data
{
    public interface IUserRepo
    {
        IEnumerable<User> GetAllUsers();
        void CreateUser(User user);
        void DeleteUser(ObjectId id);
        /*bool SaveChanges();*/
    }
}
