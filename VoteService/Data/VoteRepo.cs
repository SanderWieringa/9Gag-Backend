using VoteService.Data;
using System;
using System.ComponentModel.Design;
using VoteService.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using MongoDB.Driver;
using MongoDB.Bson;

namespace VoteService.Data
{
    public class VoteRepo : IVoteRepo
    {
        private readonly IMongoCollection<Vote> _votes;
        private readonly IMongoCollection<Post> _posts;

        public VoteRepo(IDatabaseSettings settings, IMongoClient mongoClient)
        {
            var voteDatabase = mongoClient.GetDatabase(settings.VoteDatabaseName);
            _votes = voteDatabase.GetCollection<Vote>(settings.VoteCollectionName);

            var postDatabase = mongoClient.GetDatabase(settings.PostDatabaseName);
            _posts = postDatabase.GetCollection<Post>(settings.PostCollectionName);
        }

        public void CreatePost(Post post)
        {
            if (post == null)
            {
                throw new ArgumentNullException(nameof(post));
            }
            _posts.InsertOne(post);
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
            return _posts.Find(post => true).ToList();
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
        }
    }
}
