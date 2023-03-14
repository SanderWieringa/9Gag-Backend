using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;

namespace Backend.Models
{

    public class Post
    {
        [Key]
        public int PostId { get; set; }

        public string Title { get; set; }

        // Navigation property
        [NotMapped]
        public IFormFile? Photo { get; set; }
    }

    /*public class Photo
    {
        [Key]
        public int PhotoId { get; set; }    
        public IFormFile Bytes { get; set; }

        // Reverse navigation property
        public int PostForeignKey { get; set; }
        public Post Post { get; set; }
    }*/
}
