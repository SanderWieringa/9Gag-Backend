using Microsoft.AspNetCore.SignalR;
using MongoDB.Bson;

namespace PostService.Dtos
{
    public class PostPublishedDto
    {
        public ObjectId Id { get; set; }
        public string Title { get; set; }
        public byte[] ImageFile { get; set; }
        public ObjectId UserId { get; set; }

        public string Event { get; set; }

        public PostPublishedDto(PostReadDto postReadDto)
        {
            Id = postReadDto.Id;
            Title = postReadDto.Title;
            using (var stream = postReadDto.ImageFile.OpenReadStream())
            using (var memoryStream = new MemoryStream())
            {
                stream.CopyTo(memoryStream);
                ImageFile = memoryStream.ToArray();
            }
            UserId = postReadDto.UserId;
        }
    }
}
