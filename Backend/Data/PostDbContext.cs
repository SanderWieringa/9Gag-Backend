using Backend.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace Backend.Data
{
    public class PostDbContext : DbContext
    {
        public PostDbContext(DbContextOptions<PostDbContext> opt) : base(opt)
        {

        }

        public DbSet<Post> Posts { get; set; }
    }
}
