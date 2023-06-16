using MediatR;
using MongoDB.Bson;
using PostService.Models;

namespace PostService.Commands
{
    public record DeletePostsCommand(ObjectId userId) : IRequest<ObjectId>;
}
