using PostService.Models;

namespace PostService.Data
{
    public interface IPostCollectionRepo
    {
        bool SaveChanges();

        IEnumerable<Post> GetAllPosts();
        Post GetPostById(int id);
        void CreatePost(Post post);
        Post InsertPost(Post post);
    }
}
