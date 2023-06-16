using MongoDB.Bson;
using PostService.Models;

namespace PostService.Data
{
    public interface IPostCollectionRepo
    {
        IEnumerable<Post> GetAllPosts();
        Post GetPostById(ObjectId id);
        void CreatePost(Post post);
        Post InsertPost(Post post);
        ObjectId DeletePosts(ObjectId userId);
        IEnumerable<Post> DeletePosts(IEnumerable<Post> postsToDelete);
    }
}
