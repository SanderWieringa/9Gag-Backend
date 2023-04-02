using MediatR;
using PostService.Models;

namespace PostService.Queries
{
    public record GetPostListQuery() : IRequest<List<Post>>;
}
