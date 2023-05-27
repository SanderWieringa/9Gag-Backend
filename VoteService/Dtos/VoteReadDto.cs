using MongoDB.Bson;

namespace VoteService.Dtos
{
    public class VoteReadDto
    {
        public ObjectId Id { get; set; }
        public ObjectId UserId { get; set; }
        public ObjectId PostId { get; set; }
    }
}
