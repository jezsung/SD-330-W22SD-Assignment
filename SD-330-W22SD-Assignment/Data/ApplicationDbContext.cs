using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SD_330_W22SD_Assignment.Models;

namespace SD_330_W22SD_Assignment.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Question> Questions { get; set; } = null!;
        public DbSet<Answer> Answers { get; set; } = null!;
        public DbSet<Comment> Comments { get; set; } = null!;
        public DbSet<Tag> Tags { get; set; } = null!;
        public DbSet<QuestionTag> QuestionTags { get; set; } = null!;
        public DbSet<Vote> Votes { get; set; } = null!;
        public DbSet<AnswerVote> AnswerVotes { get; set; } = null!;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<QuestionTag>().HasKey(qt => new { qt.QuestionId, qt.TagId });
            builder.Entity<Vote>().HasKey(v => new { v.QuestionId, v.UserId });
            builder.Entity<AnswerVote>().HasKey(av => new { av.AnswerId, av.UserId });
        }
    }
}