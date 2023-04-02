using PostService.Models;

namespace PostService.Data
{
    public interface IPostRepo
    {
        List<Post> GetPosts();
    }
}
