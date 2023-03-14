using Backend.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Reflection.Metadata;

namespace Backend.Data
{
    public class PostDbContext : DbContext
    {
        public PostDbContext(DbContextOptions<PostDbContext> opt) : base(opt)
        {

        }

        /*protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Post>()
                .HasOne(b => b.Photo)
                .WithOne(p => p.Post)
                .HasForeignKey<Photo>(p => p.PostForeignKey);
                

            modelBuilder.Entity<Post>()
                .Navigation(b => b.Photo)
                .UsePropertyAccessMode(PropertyAccessMode.Property);
        }*/

        public DbSet<Post> Posts { get; set; }
        /*public DbSet<Photo> Photos { get; set; }*/
    }
}
