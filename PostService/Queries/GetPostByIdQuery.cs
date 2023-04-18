using PostService.Models;
using MediatR;

namespace PostService.Queries
{
    public record GetPostByIdQuery(int id) : IRequest<Post>;
}
