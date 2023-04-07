using PostService.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PostService.Dtos
{
    public class PostCreateDto
    {
        public string Title { get; set; }
        public string Photo { get; set; }
    }
}
