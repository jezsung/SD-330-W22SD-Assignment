using Microsoft.AspNetCore.Identity;

namespace SD_330_W22SD_Assignment.Models
{
    public class ApplicationUser : IdentityUser
    {
        public int Reputation { get; set; }

        public virtual ICollection<Question> Questions { get; set; } = new List<Question>();
        public virtual ICollection<Answer> Answers { get; set; } = new List<Answer>();
        public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

        public ApplicationUser() : base()
        {

        }
    }
}
