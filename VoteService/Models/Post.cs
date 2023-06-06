using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace VoteService.Models
{
    public class Post
    {
        /*[Required]
        [Key]*/
        [BsonId]
        public ObjectId Id { get; set; }
        /*[Required]*/
        [BsonElement("externalId")]
        public ObjectId ExternalId { get; set; }
        /*[Required]*/
        [BsonElement("title")]
        public string Title { get; set; }
        /*[Required]*/
        [BsonElement("imageFile")]
        public IFormFile ImageFile { get; set; }
        public ICollection<Vote> Votes { get; set; } = new List<Vote>();
    }
}
