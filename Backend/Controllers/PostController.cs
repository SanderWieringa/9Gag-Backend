using AutoMapper;
using PostService.Commands;
using PostService.Data;
using PostService.Dtos;
using PostService.Models;
using PostService.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.IO;
using Microsoft.Extensions.Caching.Distributed;
using PostService.Extensions;

namespace PostService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly IDistributedCache _cache;
        private IEnumerable<Post> posts;
        private string loadLocation = "";

        public PostController(IMapper mapper, IMediator mediator, IDistributedCache cache)
        {
            _mapper = mapper;
            _mediator = mediator;
            _cache = cache;
        }

        [HttpGet]
        public async Task<IEnumerable<Post>> Get()
        {
            posts = null;
            loadLocation = null;

            string recordKey = "Post_" + DateTime.Now.ToString("yyyyMMdd_hhmm");

            posts = await _cache.GetRecordAsync<IEnumerable<Post>>(recordKey);

            if (posts == null)
            {
                posts = await _mediator.Send(new GetPostListQuery());

                loadLocation = $"Loaded from API at: {DateTime.Now}";
                Console.WriteLine("loadLocation = Database");
                Thread.Sleep(3000);

                await _cache.SetRecordAsync(recordKey, posts);
            }
            else
            {
                loadLocation = $"Loaded from the cache at: {DateTime.Now}";
                Console.WriteLine("loadLocation = Cache");
            }
            

            return posts;
        }

        [HttpGet("{id}")]
        public async Task<Post> Get(int id)
        {
            return await _mediator.Send(new GetPostByIdQuery(id));
        }

        [HttpPost]
        public async Task<Post> Post([FromBody] PostCreateDto postCreateDto)
        {
            var postModel = _mapper.Map<Post>(postCreateDto);
            return await _mediator.Send(new InsertPostCommand(postModel));
        }
    }
}
