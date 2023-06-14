using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace PostService.Models
{
    public class PostModel
    {
        [BsonId]
        public ObjectId Id { get; set; }
        [BsonElement("title")]
        public string Title { get; set; }
        [BsonElement("imageFile")]
        public byte[] ImageFile { get; set; }
        [BsonElement("userId")]
        public ObjectId UserId { get; set; }

        public PostModel(string title, byte[] imageFile, ObjectId userId)
        {
            Title = title;
            ImageFile = imageFile;
            UserId = userId;
        }
    }
}
