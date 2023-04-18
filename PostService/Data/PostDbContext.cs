using PostService.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Reflection.Metadata;

namespace PostService.Data
{
    public class PostDbContext : DbContext
    {
        public PostDbContext(DbContextOptions<PostDbContext> opt) : base(opt)
        {

        }

        public DbSet<Post> Posts { get; set; }
    }
}
