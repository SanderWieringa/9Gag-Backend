using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace VoteService.Models
{
    public class Post
    {
        [BsonId]
        public ObjectId Id { get; set; }
        [BsonElement("externalId")]
        public ObjectId ExternalId { get; set; }
        [BsonElement("title")]
        public string Title { get; set; }
        [BsonElement("imageFile")]
        public IFormFile ImageFile { get; set; }
        public ICollection<Vote> Votes { get; set; } = new List<Vote>();

        public Post()
        {
            
        }

        /*public Post(PostModel postModel)
        {
            Title = postModel.Title;
            ExternalId = postModel.ExternalId;
            using (var memoryStream = new MemoryStream(postModel.ImageFile))
            {
                ImageFile = new FormFile(memoryStream, 0, postModel.ImageFile.Length, null, ".png");
            }
        }*/
    }
}
