using PostService.Models;
using MediatR;
using MongoDB.Bson;

namespace PostService.Queries
{
    public record GetPostByIdQuery(ObjectId id) : IRequest<Post>;
}
