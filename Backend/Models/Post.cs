using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;

namespace Backend.Models
{
    public class Post
    {
        [Key]
        public int PostId { get; set; }

        public string Title { get; set; }

        // Navigation property
        public Photo Photo { get; set; }
    }

    public class Photo
    {
        [Key]
        public int PhotoId { get; set; }
        public byte[] Bytes { get; set; }

        [ForeignKey("PostId")]
        public int PostId { get; set; }

        // Reverse navigation property
        public Post Post { get; set; }
    }
}
