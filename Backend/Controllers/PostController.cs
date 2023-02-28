using AutoMapper;
using Backend.Data;
using Backend.Dtos;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostRepo _repository;
        private readonly IMapper _mapper;

        public PostController(IPostRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<PostReadDto>> GetPosts()
        {
            Console.WriteLine("--> Getting Posts....");

            var platformItems = _repository.GetAllPosts();

            return Ok(_mapper.Map<IEnumerable<PostReadDto>>(platformItems));
        }

        [HttpGet("{id}", Name = "GetPostById")]
        public ActionResult<PostReadDto> GetPostById(int id)
        {
            Console.WriteLine("--> Getting Post By Id....");

            var postItem = _repository.GetPostById(id);
            if (postItem != null)
            {
                return Ok(_mapper.Map<PostReadDto>(postItem));
            }

            return NotFound();
        }

        [HttpPost]
        public ActionResult<PostReadDto> CreatePost(PostCreateDto postCreateDto)
        {
            Console.WriteLine("--> Creating Post....");

            var postModel = _mapper.Map<Post>(postCreateDto);

            _repository.CreatePost(postModel);
            _repository.SaveChanges();

            var postReadDto = _mapper.Map<PostReadDto>(postModel);

            return CreatedAtRoute(nameof(GetPostById), new { Id = postReadDto.Id }, postReadDto);
        }
    }
}
