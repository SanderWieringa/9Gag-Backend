using VoteService.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace VoteService.Data
{
    public interface IVoteRepo
    {
        bool SaveChanges();


        // Posts
        IEnumerable<Post> GetAllPosts();
        void CreatePost(Post post);
        bool PostExists(int postId);
        bool ExternalPostExists(int externalPostId);

        // Votes
        IEnumerable<Vote> GetVotesForPost(int postId);
        Vote GetVote(int postId, int voteId);
        void CreateVote(int postId, Vote vote);
    }
}
