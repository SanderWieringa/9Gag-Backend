using PostService.Models;
using Microsoft.EntityFrameworkCore;
using System;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Reflection;
using System.IO;
using SharpCompress.Compressors.Xz;
using Microsoft.Extensions.Configuration.UserSecrets;

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
            if (postsToDelete.Any())
            {
                List<PostModel> postModelsToDelete = new List<PostModel>();

                foreach (var item in postsToDelete)
                {
                    if (item.ImageFile.Length > 0)
                    {
                        byte[] fileData = null;
                        using (var stream = item.ImageFile.OpenReadStream())
                        using (var memoryStream = new MemoryStream())
                        {
                            stream.CopyTo(memoryStream);
                            fileData = memoryStream.ToArray();
                            postModelsToDelete.Add(new PostModel(item.Id, item.Title, fileData, item.UserId));
                        }



                        /*PostModel post = null;*/
                        /*using (var memoryStream = item.ImageFile.OpenReadStream())
                        {
                            fileData = new byte[item.ImageFile.Length];
                            memoryStream.Read(fileData, 0, (int)item.ImageFile.Length);
                            postModelsToDelete.Add(new PostModel(item.Id, item.Title, fileData, item.UserId));
                            *//*item.ImageFile.CopyTo(memoryStream);
                            fileData = memoryStream.ToArray();
                            post = new PostModel(item.Title, fileData, item.UserId);*//*
                        }*/


                    }
                }
                var filter = Builders<PostModel>.Filter.Eq("userId", postModelsToDelete[0].UserId);
                _posts.DeleteMany(filter);
            }
            return postsToDelete;
        }

        public ObjectId DeletePosts(ObjectId userId)
        {
            var filter = Builders<PostModel>.Filter.Eq("userId", userId);
            _posts.DeleteMany(filter);

            return userId;
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
