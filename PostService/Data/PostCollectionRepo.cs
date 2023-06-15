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

                    var postDocument = new PostModel(post.Title, fileData, post.UserId);

                    _posts.InsertOne(postDocument);
                }
            }
        }

        public IEnumerable<Post> DeletePosts(IEnumerable<Post> postsToDelete)
        {
            if (_posts != null)
            {
                List<PostModel> postModelsToDelete = new List<PostModel>();
                using (var memoryStream = new MemoryStream())
                {
                    foreach (var item in postsToDelete)
                    {
                        byte[] fileData = null;

                        item.ImageFile.CopyTo(memoryStream);

                        fileData = memoryStream.ToArray();

                        postModelsToDelete.Add(new PostModel(item.Title, fileData, item.UserId));
                    }
                }

                _posts.DeleteMany(FilterDefinition<PostModel>.Empty);
            }

            return postsToDelete;
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

                    var postDocument = new PostModel(post.Title, fileData, post.UserId);

                    _posts.InsertOne(postDocument);
                }
            }
            
            return post;
        }
    }
}
