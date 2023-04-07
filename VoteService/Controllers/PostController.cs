using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VoteService.Data;
using VoteService.Dtos;

namespace VoteService.Controllers
{
    [Route("api/v/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IVoteRepo _repo;

        public PostController(IMapper mapper, IVoteRepo repo)
        {
            _mapper = mapper;
            _repo = repo;
        }

        [HttpGet]
        public ActionResult<IEnumerable<PostReadDto>> GetAllPosts()
        {
            Console.WriteLine("--> Getting Posts From VoteService");

            var postItems = _repo.GetAllPosts();

            return Ok(_mapper.Map<IEnumerable<PostReadDto>>(postItems));
        }

        [HttpPost]
        public ActionResult TestInboundConnection()
        {
            Console.WriteLine("--> Inbound POST # Vote Service");

            return Ok("Inbound test ok from Post Controller");
        }
    }
}
