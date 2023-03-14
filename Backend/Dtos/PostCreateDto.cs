using Backend.Models;
using System.ComponentModel.DataAnnotations;

namespace Backend.Dtos
{
    public class PostCreateDto
    {
        public string Title { get; set; }

        public Photo Photo { get; set; }
    }
}
