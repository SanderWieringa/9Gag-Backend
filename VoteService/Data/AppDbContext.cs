using Microsoft.EntityFrameworkCore;
using VoteService.Dtos;
using VoteService.Models;

namespace VoteService.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt)
        {
        }

        public DbSet<PostDbDto> Posts { get; set; }
        public DbSet<Vote> Votes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<PostDbDto>()
                .HasMany(p => p.Votes)
                .WithOne(p => p.Post!)
                .HasForeignKey(p => p.PostId);

            modelBuilder
                .Entity<Vote>()
                .HasOne(p => p.Post)
                .WithMany(p => p.Votes)
                .HasForeignKey(p => p.PostId);
        }
    }
}
