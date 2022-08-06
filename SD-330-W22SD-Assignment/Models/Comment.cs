using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SD_330_W22SD_Assignment.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Body { get; set; } = null!;

        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        public string UserId { get; set; } = null!;

        public int QuestionId { get; set; }

        public int? AnswerId { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; } = null!;

        [ForeignKey("QuestionId")]
        public virtual Question? Question { get; set; }

        [ForeignKey("AnswerId")]
        public virtual Answer? Answer { get; set; }
    }
}
