using Backend.Models;
using MediatR;

namespace Backend.Commands
{
   public record InsertPostCommand(Post post) : IRequest<Post>;
}
