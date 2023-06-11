using MongoDB.Bson;
using SharpCompress.Common;

namespace VoteService.Dtos
{
    public class PostPublishedDto
    {
        public ObjectId Id { get; set; }
        public string Title { get; set; }
        /*public string ImageFile { get; set; }*/
        public byte[] ImageFile { get; set; }
        /*public IFormFile ImageFile { get; set; }*/
        public string Event { get; set; }
    }
}
