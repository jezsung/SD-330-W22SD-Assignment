using System.ComponentModel.DataAnnotations;

namespace SD_330_W22SD_Assignment.Models
{
    public class Tag
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        public virtual ICollection<Question> Questions { get; set; } = new List<Question>();
    }
}
