using AuthorizationService.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace AuthorizationService.Data
{
    public class UserRepo : IUserRepo
    {
        /*private readonly AppDbContext _context;*/
        private readonly IMongoCollection<User> _users;

        public UserRepo(IDatabaseSettings settings, IMongoClient mongoClient /*AppDbContext context*/)
        {
            /*_context = context;*/
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            _users = database.GetCollection<User>(settings.CollectionName);
        }

        public void CreateUser(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            _users.InsertOne(user);
        }

        public void DeleteUser(ObjectId id)
        {
            _users.DeleteOne(user => user.id == id);
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _users.Find(user => true).ToList();
        }

        /*public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }*/
    }
}
