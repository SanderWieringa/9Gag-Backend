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

        public PostCollectionRepo(IDatabaseSettings settings, IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            _posts = database.GetCollection<PostModel>(settings.CollectionName);
        }

        public void CreatePost(Post post)
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
    }
}
