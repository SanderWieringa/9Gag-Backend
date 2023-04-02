using PostService.Models;

namespace PostService.Data
{
    public class PostRepo : IPostRepo
    {
        private readonly PostDbContext _postContext;

        public PostRepo(PostDbContext postContext)
        {
            _postContext = postContext;
        }

        public List<Post> GetPosts()
        {
            return _postContext.Posts.ToList();
        }
    }
}
