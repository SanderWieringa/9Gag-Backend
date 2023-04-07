using Backend.Models;
using MediatR;

namespace Backend.Queries
{
    public record GetPostByIdQuery(int id) : IRequest<Post>;
}
