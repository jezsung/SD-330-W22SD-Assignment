using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SD_330_W22SD_Assignment.Models
{
    public class QuestionTag
    {
        [Required]
        public int QuestionId { get; set; }

        [Required]
        public int TagId { get; set; }

        [ForeignKey("QuestionId")]
        public virtual Question Question { get; set; } = null!;

        [ForeignKey("TagId")]
        public virtual Tag Tag { get; set; } = null!;

    }
}
