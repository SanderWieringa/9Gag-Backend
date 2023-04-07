using VoteService.Models;

namespace VoteService.Data
{
    public class VoteRepo : IVoteRepo
    {
        public void CreatePost(Post post)
        {
            throw new NotImplementedException();
        }

        public void CreateVote(int postId, Vote vote)
        {
            throw new NotImplementedException();
        }

        public bool ExternalPostExists(int externalPostId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Post> GetAllPosts()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Vote> GetVotesForPost(int postId)
        {
            throw new NotImplementedException();
        }

        public bool PostExists(int postId)
        {
            throw new NotImplementedException();
        }

        public bool SaveChanges()
        {
            throw new NotImplementedException();
        }
    }
}
