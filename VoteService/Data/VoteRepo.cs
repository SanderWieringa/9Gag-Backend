using VoteService.Data;
using System;
using System.ComponentModel.Design;
using VoteService.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using MongoDB.Driver;
using MongoDB.Bson;
using VoteService.Dtos;

namespace VoteService.Data
{
    public class VoteRepo : IVoteRepo
    {
        /*private readonly IMongoCollection<Vote> _votes;
        private readonly IMongoCollection<PostModel> _posts;*/
        private readonly AppDbContext _context;

        public VoteRepo(/*IDatabaseSettings settings, IMongoClient mongoClient*/ AppDbContext context)
        {
            /*var voteDatabase = mongoClient.GetDatabase(settings.VoteDatabaseName);
            _votes = voteDatabase.GetCollection<Vote>(settings.VoteCollectionName);

            var postDatabase = mongoClient.GetDatabase(settings.PostDatabaseName);
            _posts = postDatabase.GetCollection<PostModel>(settings.PostCollectionName);*/
            _context = context;
        }

        public void CreatePost(PostDbDto post)
        {
            if (post == null)
            {
                throw new ArgumentNullException(nameof(post));
            }
            _context.Posts.Add(post);
        }

        public void CreateVote(ObjectId postId, Vote vote)
        {
            if (vote == null)
            {
                throw new ArgumentNullException(nameof(vote));
            }

            vote.PostId = postId;
            _context.Votes.Add(vote);
        }

        public bool ExternalPostExists(ObjectId externalPostId)
        {
            return _context.Posts.Any(p => p.ExternalId == externalPostId);
        }

        public IEnumerable<PostDbDto> GetAllPosts()
        {
            return _context.Posts.ToList();
        }

        public Vote GetVote(ObjectId postId, ObjectId voteId)
        {
            return _context.Votes.Where(v => v.PostId == postId && v.Id == voteId).FirstOrDefault();
        }

        public IEnumerable<Vote> GetVotesForPost(ObjectId postId)
        {
            return _context.Votes.Where(v => v.PostId == postId);
        }

        public bool PostExists(ObjectId postId)
        {
            return _context.Posts.Any(p => p.Id == postId);
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }

        /*public void CreatePost(Post post)
        {
            *//*if (post == null)
            {
                throw new ArgumentNullException(nameof(post));
            }
            _posts.InsertOne(post);*//*
            if (post.ImageFile != null && post.ImageFile.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    post.ImageFile.CopyToAsync(memoryStream);

                    // Reset the position of the MemoryStream to the beginning
                    memoryStream.Position = 0;

                    var fileData = memoryStream.ToArray();

                    var postDocument = new PostModel(*//*post.ExternalId, *//*post.Title, fileData);

                    _posts.InsertOne(postDocument);
                }
            }
        }

        public void CreateVote(ObjectId postId, Vote vote)
        {
            if (vote == null)
            {
                throw new ArgumentNullException(nameof(vote));
            }

            vote.PostId = postId;
            _votes.InsertOne(vote);
        }

        public bool ExternalPostExists(ObjectId externalPostId)
        {
            var externalPostExists = _posts.Find(post => post.ExternalId == externalPostId).FirstOrDefault();
            if (externalPostExists != null)
            {
                return true;
            }
            return false;

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

        public Vote GetVote(ObjectId postId, ObjectId voteId)
        {
            return _votes.Find(v => v.PostId == postId && v.Id == voteId).FirstOrDefault();
        }

        public IEnumerable<Vote> GetVotesForPost(ObjectId postId)
        {
            return _votes.Find(v => v.PostId == postId).ToList();
        }

        public bool PostExists(ObjectId postId)
        {
            var postExists = _posts.Find(p => p.Id == postId);
            if (postExists != null)
            {
                return true;
            }
            return false;
        }*/
    }
}
