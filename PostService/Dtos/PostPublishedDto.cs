using MongoDB.Bson;

namespace PostService.Dtos
{
    public class PostPublishedDto
    {
        public ObjectId Id { get; set; }
        public string Title { get; set; }

        public IFormFile ImageFile { get; set; }

        public string Event { get; set; }
    }
}
