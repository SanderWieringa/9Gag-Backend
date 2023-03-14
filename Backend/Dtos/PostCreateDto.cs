using Backend.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Backend.Dtos
{
    public class PostCreateDto
    {
        public string Title { get; set; }
        public IFormFile Photo { get; set; }
    }
}
