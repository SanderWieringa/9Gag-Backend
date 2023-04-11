using AutoMapper;
using Backend.Commands;
using Backend.Data;
using Backend.Dtos;
using Backend.Models;
using Backend.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.IO;
using Microsoft.Extensions.Caching.Distributed;
using Backend.Extensions;
using PostService.AsyncDataServices;
using PostService.Dtos;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly IDistributedCache _cache;
        private readonly IMessageBusClient _messageBusClient;
        private IEnumerable<Post> posts;
        private string loadLocation = "";
        

        public PostController(IMapper mapper, IMediator mediator, IDistributedCache cache, IMessageBusClient messageBusClient)
        {
            _mapper = mapper;
            _mediator = mediator;
            _cache = cache;
            _messageBusClient = messageBusClient;
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

            Post post = await _mediator.Send(new InsertPostCommand(postModel));

            var postReadDto = _mapper.Map<PostReadDto>(postModel);

            // Send Async Message
            try
            {
                var platformPublishedDto = _mapper.Map<PostPublishedDto>(postReadDto);
                platformPublishedDto.Event = "Post_Published";
                _messageBusClient.PublishNewPost(platformPublishedDto);
            }
            catch (Exception e)
            {
                Console.WriteLine($"--> Could not send asynchronously: {e.Message}");
            }
            return post;
        }
    }
}
