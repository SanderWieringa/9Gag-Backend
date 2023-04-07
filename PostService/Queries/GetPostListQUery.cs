using Backend.Models;
using MediatR;

namespace Backend.Queries
{
    public record GetPostListQuery() : IRequest<IEnumerable<Post>>;
}
