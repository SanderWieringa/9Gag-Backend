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

        public PostModel(string title, byte[] imageFile)
        {
            Title = title;
            ImageFile = imageFile;
        }
    }
}
