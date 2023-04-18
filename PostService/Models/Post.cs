using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;

namespace PostService.Models
{

    public class Post
    {
        [Key]
        public int Id { get; set; }

        public string Title { get; set; }

        public string Photo { get; set; }
    }
}
