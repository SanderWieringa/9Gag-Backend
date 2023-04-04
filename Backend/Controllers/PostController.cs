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

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostCollectionRepo _repository;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public PostController(IPostCollectionRepo repository, IMapper mapper, IMediator mediator)
        {
            _repository = repository;
            _mapper = mapper;
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IEnumerable<Post>> Get()
        {
            return await _mediator.Send(new GetPostListQuery());
        }

        /*[HttpGet]
        public ActionResult<IEnumerable<PostReadDto>> GetPosts()
        {
            Console.WriteLine("--> Getting Posts....");

            var platformItems = _repository.GetAllPosts();

            return Ok(_mapper.Map<IEnumerable<PostReadDto>>(platformItems));
        }*/

        [HttpGet("{id}")]
        public async Task<Post> Get(int id)
        {
            return await _mediator.Send(new GetPostByIdQuery(id));
        }

        /*[HttpGet("{id}", Name = "GetPostById")]
        public ActionResult<PostReadDto> GetPostById(int id)
        {
            Console.WriteLine("--> Getting Post By Id....");

            var postItem = _repository.GetPostById(id);
            if (postItem != null)
            {
                return Ok(_mapper.Map<PostReadDto>(postItem));
            }

            return NotFound();
        }*/

        [HttpPost]
        public async Task<Post> Post([FromBody] PostCreateDto postCreateDto)
        {
            var postModel = _mapper.Map<Post>(postCreateDto);
            return await _mediator.Send(new InsertPostCommand(postModel));
        }

        /*[HttpPost]
        public ActionResult<PostReadDto> CreatePost([FromForm] PostCreateDto postCreateDto)
        {
            Console.WriteLine("--> Creating Post....");

            var postModel = _mapper.Map<Post>(postCreateDto);

            _repository.CreatePost(postModel);
            _repository.SaveChanges();

            var postReadDto = _mapper.Map<PostReadDto>(postModel);

            return CreatedAtRoute(nameof(GetPostById), new { Id = postReadDto.Id }, postReadDto);
        }*/
    }
}
