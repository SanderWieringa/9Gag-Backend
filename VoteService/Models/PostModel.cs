using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using VoteService.Dtos;

namespace VoteService.Models
{
    public class PostModel
    {
        [BsonId]
        public ObjectId Id { get; set; }
        /*[BsonElement("externalId")]
        public ObjectId ExternalId { get; set; }*/
        [BsonElement("title")]
        public string Title { get; set; }
        [BsonElement("imageFile")]
        public IFormFile ImageFile { get; set; }

        public PostModel(PostPublishedDto postPublishedDto)
        {
            Id = postPublishedDto.Id;
            Title = postPublishedDto.Title;
            using (var memoryStream = new MemoryStream(postPublishedDto.ImageFile))
            {
                ImageFile = new FormFile(memoryStream, 0, postPublishedDto.ImageFile.Length, null, ".png");
            }
        }

        /*public PostModel(*//*ObjectId externalId, *//*string title, byte[] imageFile)
        {
            *//*ExternalId = externalId;*//*
            Title = title;
            ImageFile = imageFile;
        }*/
    }
}
