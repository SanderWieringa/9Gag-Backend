using MediatR;
using PostService.Commands;
using PostService.Data;
using PostService.Models;

namespace PostService.Handlers
{
    public class DeletePostListHandler : IRequestHandler<DeletePostListCommand, IEnumerable<Post>>
    {
        private readonly IPostCollectionRepo _repo;

        public DeletePostListHandler(IPostCollectionRepo repo)
        {
            _repo = repo;
        }

        public Task<IEnumerable<Post>> Handle(DeletePostListCommand request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_repo.DeletePosts(request.postList));
        }
    }
}
