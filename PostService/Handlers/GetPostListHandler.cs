using MediatR;
using PostService.Data;
using PostService.Models;
using PostService.Queries;

namespace PostService.Handlers
{
    public class GetPostListHandler : IRequestHandler<GetPostListQuery, List<Post>>
    {
        private readonly IPostRepo _repo;

        public GetPostListHandler(IPostRepo repo)
        {
            _repo = repo;
        }

        public Task<List<Post>> Handle(GetPostListQuery request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_repo.GetPosts());
        }
    }
}
