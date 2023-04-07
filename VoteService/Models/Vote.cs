using System.ComponentModel.DataAnnotations;

namespace VoteService.Models
{
    public class Vote
    {
        [Required]
        [Key]
        public int Id { get; set; }
        [Required]
        public int PostId { get; set; }
        public Post Post { get; set; }
    }
}
