using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SD_330_W22SD_Assignment.Models
{
    public class AnswerVote
    {
        [Required]
        public int AnswerId { get; set; }
        
        [Required]

        public string UserId { get; set; } = null!;

        [Required]
        public bool Up { get; set; }

        [ForeignKey("AnswerId")]
        public virtual Answer Answer { get; set; } = null!;

        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; } = null!;
    }
}
