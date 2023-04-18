using PostService.Models;
using MediatR;

namespace PostService.Queries
{
    public record GetPostListQuery() : IRequest<IEnumerable<Post>>;
}
