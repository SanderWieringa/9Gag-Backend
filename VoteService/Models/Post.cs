using System.ComponentModel.DataAnnotations;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace VoteService.Models
{
    public class Post
    {
        [Required]
        [Key]
        public int Id { get; set; }
        [Required]
        public int ExternalId { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Photo { get; set; }
        public ICollection<Vote> Votes { get; set; } = new List<Vote>();
    }
}
