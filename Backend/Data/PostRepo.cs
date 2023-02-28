using Backend.Models;
using System;

namespace Backend.Data
{
    public class PostRepo : IPostRepo
    {
        private readonly PostDbContext _context;

        public PostRepo(PostDbContext context)
        {
            _context = context;
        }

        public void CreatePost(Post post)
        {
            if (post == null)
            {
                throw new ArgumentNullException(nameof(post));
            }

            _context.Posts.Add(post);
        }

        public IEnumerable<Post> GetAllPosts()
        {
            return _context.Posts.ToList();
        }

        public Post GetPostById(int id)
        {
            return _context.Posts.FirstOrDefault(p => p.Id == id);
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }
    }
}
