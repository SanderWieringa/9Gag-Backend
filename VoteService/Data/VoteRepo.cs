using CommandService.Data;
using System;
using System.ComponentModel.Design;
using VoteService.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace VoteService.Data
{
    public class VoteRepo : IVoteRepo
    {
        private readonly AppDbContext _context;

        public VoteRepo(AppDbContext context)
        {
            _context = context;
        }

        public void CreatePost(Post post)
        {
            if (post == null)
            {
                throw new ArgumentNullException(nameof(post));
            }
            _context.Posts.Add(post);
        }

        public void CreateVote(int postId, Vote vote)
        {
            if (vote == null)
            {
                throw new ArgumentNullException(nameof(vote));
            }

            vote.PostId = postId;
            _context.Votes.Add(vote);
        }

        public bool ExternalPostExists(int externalPostId)
        {
            return _context.Posts.Any(p => p.ExternalId == externalPostId);
        }

        public IEnumerable<Post> GetAllPosts()
        {
            return _context.Posts.ToList();
        }

        public Vote GetVote(int postId, int voteId)
        {
            return _context.Votes
                .Where(v => v.PostId == postId && v.Id == voteId).FirstOrDefault();
        }

        public IEnumerable<Vote> GetVotesForPost(int postId)
        {
            return _context.Votes
                .Where(v => v.PostId == postId);
        }

        public bool PostExists(int postId)
        {
            return _context.Posts.Any(p => p.Id == postId);
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }
    }
}
