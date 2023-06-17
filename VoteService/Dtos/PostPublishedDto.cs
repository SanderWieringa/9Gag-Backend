using MongoDB.Bson;
using SharpCompress.Common;

namespace VoteService.Dtos
{
    public class PostPublishedDto
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public byte[] ImageFile { get; set; }
        public string UserId { get; set; }
        public string Event { get; set; }
    }
}
