using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;
using VoteService.Models;

namespace CommandService.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt)
        {
        }

        public DbSet<Post> Posts { get; set; }
        public DbSet<Vote> Votes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Post>()
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
