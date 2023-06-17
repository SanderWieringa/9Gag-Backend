using PostService.Commands;
using PostService.Data;
using PostService.Models;
using MediatR;

namespace PostService.Handlers
{
    public class InsertPostHandler : IRequestHandler<InsertPostCommand, Post>
    {
        private readonly IPostCollectionRepo _repo;

        public InsertPostHandler(IPostCollectionRepo repo)
        {
            _repo = repo;
        }

        public Task<Post> Handle(InsertPostCommand request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_repo.InsertPost(request.post));
        }
    }
}
