using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using VoteService.Models;

namespace VoteService.Dtos
{
    public class PostDbDto
    {
        [BsonId]
        public ObjectId Id { get; set; }
        [BsonElement("externalId")]
        public ObjectId ExternalId { get; set; }
        [BsonElement("title")]
        public string Title { get; set; }
        [BsonElement("imageFile")]
        public byte[] ImageFile { get; set; }
        public ICollection<Vote> Votes { get; set; } = new List<Vote>();

        public PostDbDto(Post post)
        {
            Id = post.Id;
            ExternalId = post.ExternalId;
            Title = post.Title;
            using (var memoryStream = new MemoryStream())
            {
                post.ImageFile.CopyToAsync(memoryStream).Wait();
                memoryStream.Position = 0;
                ImageFile = memoryStream.ToArray();
            }
        }

        public PostDbDto()
        {
            
        }

        /*public PostDbDto(PostPublishedDto post)
        {
            Id = post.Id;
            ExternalId = post.ExternalId;
            Title = post.Title;
            using (var memoryStream = new MemoryStream())
            {
                post.ImageFile.CopyToAsync(memoryStream).Wait();
                memoryStream.Position = 0;
                ImageFile = memoryStream.ToArray();
            }
        }*/
    }
}
