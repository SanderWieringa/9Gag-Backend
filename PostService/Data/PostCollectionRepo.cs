using PostService.Models;
using Microsoft.EntityFrameworkCore;
using System;
using MongoDB.Driver;
using MongoDB.Bson;

namespace PostService.Data
{
    public class PostCollectionRepo : IPostCollectionRepo
    {
        private readonly IMongoCollection<Post> _posts;

        /*private readonly PostDbContext _context;*/

        public PostCollectionRepo(IDatabaseSettings settings, IMongoClient mongoClient /*PostDbContext context*/)
        {
            /*_context = context;*/
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            _posts = database.GetCollection<Post>(settings.CollectionName);
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
            _posts.InsertOne(post);
        }

        public IEnumerable<Post> GetAllPosts()
        {
            return _posts.Find(post => true).ToList();
        }

        public Post GetPostById(ObjectId id)
        {
            return _posts.Find(post => post.Id == id).FirstOrDefault();
        }

        public Post InsertPost(Post post)
        {
            _posts.InsertOne(post);
            return post;
        }

        /*
        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }*/
    }
}
