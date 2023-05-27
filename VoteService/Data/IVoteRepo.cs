using MongoDB.Bson;
using VoteService.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace VoteService.Data
{
    public interface IVoteRepo
    {
        /*bool SaveChanges();*/


        // Posts
        IEnumerable<Post> GetAllPosts();
        void CreatePost(Post post);
        bool PostExists(ObjectId postId);
        bool ExternalPostExists(ObjectId externalPostId);

        // Votes
        IEnumerable<Vote> GetVotesForPost(ObjectId postId);
        Vote GetVote(ObjectId postId, ObjectId voteId);
        void CreateVote(ObjectId postId, Vote vote);
    }
}
