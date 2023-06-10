using MongoDB.Bson;

namespace VoteService.Dtos
{
    public class PostReadDto
    {
        public ObjectId Id { get; set; }
        public string Title { get; set; }
        public IFormFile ImageFile { get; set; }
    }
}
