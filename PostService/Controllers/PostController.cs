using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PostService.Models;
using PostService.Queries;

namespace PostService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PostController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<List<Post>> GetPosts()
        {
            return await _mediator.Send(new GetPostListQuery());
        }

        [HttpPost]
        public async Task<Post> CreatePost([FromBody] Post newPost)
        {
            return await _mediator.Send(new InsertPersonCommand(newPost));
        }
    }
}
