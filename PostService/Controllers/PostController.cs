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
using PostService.AsyncDataServices;
using PostService.Dtos;
using MongoDB.Bson;
using StackExchange.Redis;
using Microsoft.Extensions.Configuration;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace PostService.Controllers
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
        public IConfiguration Configuration { get; }
        private readonly IWebHostEnvironment _hostEnvironment;

        ConfigurationOptions options = null;
        ConnectionMultiplexer redis = null;

        public PostController(IConfiguration configuration, IMapper mapper, IMediator mediator, IDistributedCache cache, IMessageBusClient messageBusClient, IWebHostEnvironment hostEnvironment)
        {
            Configuration = configuration;
            _mapper = mapper;
            _mediator = mediator;
            _cache = cache;
            _messageBusClient = messageBusClient;
            _hostEnvironment = hostEnvironment;
            options = new ConfigurationOptions
            {
                EndPoints = { { Configuration.GetConnectionString("Redis"), 10967 } },
                Password = "wmKX4BRxr7GA!",
                User = "9GagUser",
                Ssl = true, // Set to true if using SSL/TLS encryption
                AbortOnConnectFail = false // Set to true to throw an exception on connection failure
            };
            redis = ConnectionMultiplexer.Connect(Configuration.GetConnectionString("Redis"));
        }


        [HttpGet]
        public async Task<IEnumerable<Post>> Get()
        {
            List<Post> posts = null;
            //ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(Configuration.GetConnectionString("Redis"));

            IDatabase redisDb = redis.GetDatabase();

            // Use SCAN command to iterate over all keys in Redis
            string cursor = "0";

            // Scan for keys matching a pattern (e.g., "*" for all keys)
            RedisResult scanResult = redisDb.Execute("SCAN", cursor);

            // Retrieve the cursor and the keys from the scan result
            RedisResult[] scanArray = (RedisResult[])scanResult;
            cursor = (string)scanArray[0];
            RedisKey[] keys = (RedisKey[])scanArray[1];

            // Use GET command to fetch the values of the keys
            RedisValue[] values = redisDb.StringGet(keys);

            // Process the retrieved values as desired
            foreach (RedisValue value in values)
            {
                // Do something with the value
                Console.WriteLine(value);
            }

            redis.Close();
            redis.Dispose();

            return posts;
        }

        [HttpGet("{id}")]
        public async Task<Post> Get(ObjectId id)
        {
            return await _mediator.Send(new GetPostByIdQuery(id));
        }

        [HttpPost]
        [Authorize]
        [Consumes("multipart/form-data")]
        public async Task<Post> Post([FromForm] PostCreateDto postCreateDto)
        {
            await SaveImage(postCreateDto.ImageFile);
            Post postModel = ConvertToPost(postCreateDto);

            Post post = await _mediator.Send(new InsertPostCommand(postModel));

            var postReadDto = _mapper.Map<PostReadDto>(postModel);

            // Send Async Message
            try
            {
                PostPublishedDto postPublishedDto = new PostPublishedDto(postReadDto);
                /*var platformPublishedDto = _mapper.Map<PostPublishedDto>(postReadDto);*/
                postPublishedDto.Event = "Post_Published";
                _messageBusClient.PublishNewPost(postPublishedDto);
            }
            catch (Exception e)
            {
                Console.WriteLine($"--> Could not send asynchronously: {e.Message}");
            }
            return post;
        }

        private Post ConvertToPost(PostCreateDto postCreateDto)
        {
            Post post = new Post(postCreateDto.Title, postCreateDto.ImageFile);

            return post;
        }

        [NonAction]
        public async Task<string> SaveImage(IFormFile imageFile)
        {
            string imageName = new String(Path.GetFileNameWithoutExtension(imageFile.FileName).Take(10).ToArray()).Replace(' ', '-');
            imageName = imageName + DateTime.Now.ToString("yymmssfff") + Path.GetExtension(imageFile.FileName);
            var imagePath = Path.Combine(_hostEnvironment.ContentRootPath, "Images", imageName);

            using (var fileStream = new FileStream(imagePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStream);
            }

            return imageName;
        }
    }
}
