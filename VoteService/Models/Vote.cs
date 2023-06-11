using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using VoteService.Dtos;

namespace VoteService.Models
{
    public class Vote
    {
        [BsonId]
        public ObjectId Id { get; set; }
        [BsonElement("userId")]
        public ObjectId UserId { get; set; }
        [BsonElement("postId")]
        public ObjectId PostId { get; set; }
        public PostDbDto Post { get; set; }
    }
}
