using Backend.Models;

namespace Backend.Data
{
    public interface IPostRepo
    {
        bool SaveChanges();

        IEnumerable<Post> GetAllPosts();
        Post GetPostById(int id);
        void CreatePost(Post post);
    }
}
