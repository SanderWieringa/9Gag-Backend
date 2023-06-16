using MediatR;
using MongoDB.Bson;
using PostService.Commands;
using PostService.Data;
using PostService.Models;

namespace PostService.Handlers
{
    public class DeletePostsHandler : IRequestHandler<DeletePostsCommand, ObjectId>
    {
        private readonly IPostCollectionRepo _repo;

        public DeletePostsHandler(IPostCollectionRepo repo)
        {
            _repo = repo;
        }

        public Task<ObjectId> Handle(DeletePostsCommand request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_repo.DeletePosts(request.userId));
        }

        /*public ObjectId Handle(DeletePostsCommand request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_repo.DeletePosts(request.userId));
        }

        Task<ObjectId> IRequestHandler<DeletePostsCommand, ObjectId>.Handle(DeletePostsCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }*/
    }
}
