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
using System.Net;

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

        // fix 1
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

        /* [HttpPost]
         public async Task<IHttpActionResult> UploadImage()
         {
             // Check if the request contains multipart/form-data
             if (!Request.Content.IsMimeMultipartContent())
             {
                 return StatusCode((int)HttpStatusCode.UnsupportedMediaType);
             }

             try
             {
                 // Set the folder path to save the uploaded image
                 string folderPath = "C:/Uploads";
                 Directory.CreateDirectory(folderPath);

                 // Create a stream provider to process the multipart/form-data request
                 var provider = new MultipartFormDataStreamProvider(folderPath);

                 // Read the request content and process the multipart/form-data asynchronously
                 await Request.Content.ReadAsMultipartAsync(provider);

                 // Get the uploaded image file from the provider's file data
                 var fileData = provider.FileData[0];
                 var originalFileName = fileData.Headers.ContentDisposition.FileName.Trim('\"');

                 // Generate a unique file name to save the image
                 string uniqueFileName = Path.GetFileNameWithoutExtension(originalFileName)
                     + "_" + Path.GetRandomFileName().Substring(0, 8)
                     + Path.GetExtension(originalFileName);

                 // Move the uploaded image to the final destination with the unique file name
                 string filePath = Path.Combine(folderPath, uniqueFileName);
                 File.Move(fileData.LocalFileName, filePath);

                 // Optionally, perform additional operations on the image file

                 // Return a success response with the file path or any other relevant information
                 return Ok(filePath);
             }
             catch (System.Exception ex)
             {
                 // Handle any exceptions that occur during the upload process
                 return InternalServerError(ex);
             }
         }*/

        /*[HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadImage([FromForm] ImageUploadModel model)
        {
            if (model == null || model.Image == null || model.Image.Length == 0)
            {
                return BadRequest("No image data provided");
            }

            try
            {
                // Validate the image or perform additional checks if needed

                // Generate a unique file name to save the image
                string uniqueFileName = $"{Guid.NewGuid()}{Path.GetExtension(model.Image.FileName)}";

                // Set the path to save the uploaded image
                string filePath = Path.Combine("C:/Uploads", uniqueFileName);

                // Save the image file
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.Image.CopyToAsync(stream);
                }

                // Optionally, perform additional operations on the image file

                // Return a success response with the file path or any other relevant information
                return Ok(filePath);
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during the upload process
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while uploading the image");
            }
        }*/

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<Post> Post([FromForm] PostCreateDto postCreateDto)
        {
            /*PostCreateDto postCreateDto = new();*/
            await SaveImage(postCreateDto.ImageFile);
            //var postModel = _mapper.Map<Post>(postCreateDto);
            Post postModel = ConvertToPost(postCreateDto);

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

        private Post ConvertToPost(PostCreateDto postCreateDto)
        {
            Post post = new Post(postCreateDto.Title, postCreateDto.ImageFile);
            /*post.Title = postCreateDto.Title;
            post.ImageFile = postCreateDto.ImageFile;*/

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
