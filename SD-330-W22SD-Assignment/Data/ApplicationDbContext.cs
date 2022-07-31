using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SD_330_W22SD_Assignment.Models;

namespace SD_330_W22SD_Assignment.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public virtual DbSet<Question> Questions { get; set; } = null!;
        public virtual DbSet<Answer> Answers { get; set; } = null!;
        public virtual DbSet<Comment> Comment { get; set; } = null!;
        public virtual DbSet<Tag> Tags { get; set; } = null!;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}