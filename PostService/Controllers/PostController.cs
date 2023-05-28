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
using PostService.AsyncDataServices;
using PostService.Dtos;
using MongoDB.Bson;
using StackExchange.Redis;
using Microsoft.Extensions.Configuration;

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
        // fix 1
        ConfigurationOptions options = null;
        ConnectionMultiplexer redis = null;

        public PostController(IConfiguration configuration, IMapper mapper, IMediator mediator, IDistributedCache cache, IMessageBusClient messageBusClient)
        {
            Configuration = configuration;
            _mapper = mapper;
            _mediator = mediator;
            _cache = cache;
            _messageBusClient = messageBusClient;
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

            /*posts = null;
            loadLocation = null;

            string recordKey = "Post_" + DateTime.Now.ToString("yyyyMMdd_hhmm");
            try {
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
            }
            catch(Exception e) {
                Console.WriteLine(e.Message);
            }

            return posts;*/
        }

        [HttpGet("{id}")]
        public async Task<Post> Get(ObjectId id)
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
