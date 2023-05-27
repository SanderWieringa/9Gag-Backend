using MongoDB.Bson;

namespace AuthorizationService.Models
{
    public class User
    {
        public ObjectId id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string oauthSubject { get; set; }
        public string oauthIssuer { get; set; }
    }
}
