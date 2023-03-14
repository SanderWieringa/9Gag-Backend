using Backend.Models;

namespace Backend.Dtos
{
    public class PostReadDto
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public Photo Photo { get; set; }
    }
}
