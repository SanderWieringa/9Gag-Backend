using Backend.Commands;
using Backend.Data;
using Backend.Models;
using MediatR;

namespace Backend.Handlers
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
            return Task.FromResult(this._repo.InsertPost(request.post));
        }
    }
}
