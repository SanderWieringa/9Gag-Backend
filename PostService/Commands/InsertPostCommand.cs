using PostService.Models;
using MediatR;

namespace PostService.Commands
{
   public record InsertPostCommand(Post post) : IRequest<Post>;
}
