using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace VoteService.Models
{
    public class Vote
    {
        /*[Required]
        [Key]*/
        [BsonId]
        public ObjectId Id { get; set; }
        [BsonElement("userId")]
        public ObjectId UserId { get; set; }
        [BsonElement("postId")]
        public ObjectId PostId { get; set; }
        public Post Post { get; set; }
    }
}
