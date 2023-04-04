using Backend.Models;
using Backend.Queries;
using MediatR;

namespace Backend.Handlers
{
    public class GetPostByIdHandler : IRequestHandler<GetPostByIdQuery, Post>
    {
        private readonly IMediator _mediator;

        public GetPostByIdHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Post> Handle(GetPostByIdQuery request, CancellationToken cancellationToken)
        {
            var results = await _mediator.Send(new GetPostListQuery());

            var output = results.FirstOrDefault(x => x.Id == request.id);

            return output;
        }
    }
}
