using PostService.Models;
using Microsoft.EntityFrameworkCore;
using System;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Reflection;

namespace PostService.Data
{
    public class PostCollectionRepo : IPostCollectionRepo
    {
        private readonly IMongoCollection<PostModel> _posts;

        /*private readonly PostDbContext _context;*/

        public PostCollectionRepo(IDatabaseSettings settings, IMongoClient mongoClient /*PostDbContext context*/)
        {
            /*_context = context;*/
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            _posts = database.GetCollection<PostModel>(settings.CollectionName);
        }

        public void CreatePost(Post post)
        {
            /*if (post == null)
            {
                throw new ArgumentNullException(nameof(post));
            }
            *//*using (var ms = new MemoryStream())
            {
                post.Photo.CopyTo(ms);
                var fileBytes = ms.ToArray();
                string bytes = Convert.ToBase64String(fileBytes);
                post.Image = bytes;
            }*/
            if (post.ImageFile != null && post.ImageFile.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    post.ImageFile.CopyTo(memoryStream);

                    var fileData = memoryStream.ToArray();

                    var postDocument = new PostModel(post.Title, fileData);

                    _posts.InsertOne(postDocument);
                }
            }
        }

        public IEnumerable<Post> GetAllPosts()
        {
            var postModelList = _posts.Find(post => true).ToList();
            List<Post> posts = new List<Post>();
            if (postModelList.Count > 0)
            {
                foreach (var item in postModelList)
                {
                    posts.Add(new Post(item));
                }
            }

            return posts;
        }

        //demo
        public Post GetPostById(ObjectId id)
        {
            var postModel = _posts.Find(post => post.Id == id).FirstOrDefault();
            if (postModel != null)
            {
                return new Post(postModel);
            }

            return default;
        }

        public Post InsertPost(Post post)
        {
            if (post.ImageFile != null && post.ImageFile.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    post.ImageFile.CopyTo(memoryStream);

                    var fileData = memoryStream.ToArray();

                    var postDocument = new PostModel(post.Title, fileData);

                    _posts.InsertOne(postDocument);
                }
            }
            
            return post;
        }

        /*
        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }*/
    }
}
