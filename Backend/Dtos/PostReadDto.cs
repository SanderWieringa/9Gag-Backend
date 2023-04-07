using PostService.Models;
using System.Text.Json.Serialization;

namespace PostService.Dtos
{
    public class PostReadDto
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Image { get; set; }
    }
}
