using MongoDB.Bson;
using PostService.Models;
using System.Text.Json.Serialization;

namespace PostService.Dtos
{
    public class PostReadDto
    {
        public ObjectId Id { get; set; }

        public string Title { get; set; }

        public string Photo { get; set; }
    }
}
