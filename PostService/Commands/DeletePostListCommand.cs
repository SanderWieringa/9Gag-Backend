using MediatR;
using PostService.Models;

namespace PostService.Commands
{
    public record DeletePostListCommand(IEnumerable<Post> postList) : IRequest<IEnumerable<Post>>;
}
