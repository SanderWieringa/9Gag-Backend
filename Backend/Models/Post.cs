using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace Backend.Models
{
    public class Post
    {
        [Key]
        [Required]
        public int Id { get; set; }

        public string? Title { get; set; }
    }
}
