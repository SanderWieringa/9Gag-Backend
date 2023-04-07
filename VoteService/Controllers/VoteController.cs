using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VoteService.Data;

namespace VoteService.Controllers
{
    [Route("api/v/Post/{postId}/[controller]")]
    [ApiController]
    public class VoteController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IVoteRepo _repo;

        public VoteController(IMapper mapper, IVoteRepo repo)
        {
            _mapper = mapper;
            _repo = repo;
        }
    }
}
