using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SD_330_W22SD_Assignment.Models
{
    public class Question
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = null!;

        [Required]
        public string Body { get; set; } = null!;

        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        public string UserId { get; set; } = null!;

        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; } = null!;

        public virtual ICollection<Answer> Answers { get; set; } = new List<Answer>();

        public virtual ICollection<Tag> Tags { get; set; } = new List<Tag>();

        public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}
