using MediatR;
using PostService.Models;

namespace PostService.Commands
{
    public record InsertPostCommand(Post post) : IRequest<Post>;
}
