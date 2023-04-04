using Backend.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace Backend.Data
{
    public class PostCollectionRepo : IPostCollectionRepo
    {
        private readonly PostDbContext _context;

        public PostCollectionRepo(PostDbContext context)
        {
            _context = context;
        }

        public void CreatePost(Post post)
        {
            if (post == null)
            {
                throw new ArgumentNullException(nameof(post));
            }
            /*using (var ms = new MemoryStream())
            {
                post.Photo.CopyTo(ms);
                var fileBytes = ms.ToArray();
                string bytes = Convert.ToBase64String(fileBytes);
                post.Image = bytes;
            }*/
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

        public Post InsertPost(Post post)
        {
            _context.Posts.Add(post);
            _context.SaveChanges();
            return post;
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }
    }
}
