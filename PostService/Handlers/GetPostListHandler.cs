using Backend.Data;
using Backend.Models;
using Backend.Queries;
using MediatR;

namespace Backend.Handlers
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
