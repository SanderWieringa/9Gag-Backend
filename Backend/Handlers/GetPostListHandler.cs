using PostService.Data;
using PostService.Models;
using PostService.Queries;
using MediatR;

namespace PostService.Handlers
{
    public class GetPostListHandler : IRequestHandler<GetPostListQuery, IEnumerable<Post>>
    {
        private readonly IPostCollectionRepo _data;

        public GetPostListHandler(IPostCollectionRepo data)
        {
            _data = data;
        }

        public Task<IEnumerable<Post>> Handle(GetPostListQuery request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_data.GetAllPosts());
        }
    }
}
