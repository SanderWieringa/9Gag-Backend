using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using VoteService.Data;
using VoteService.Dtos;
using VoteService.Models;

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

        [HttpGet]
        public ActionResult<IEnumerable<VoteReadDto>> GetVotesForPost(ObjectId postId)
        {
            Console.WriteLine($"--> Hit GetVotesForPost: {postId}");

            if (!_repo.PostExists(postId))
            {
                return NotFound();
            }

            var votes = _repo.GetVotesForPost(postId);

            return Ok(_mapper.Map<IEnumerable<VoteReadDto>>(votes));
        }

        [HttpGet("{voteId}", Name = "GetVoteForPost")]
        public ActionResult<VoteReadDto> GetVoteForPost(ObjectId postId, ObjectId voteId)
        {
            Console.WriteLine($"--> Hit GetVoteForPost: {postId} / {voteId}");

            if (!_repo.PostExists(postId))
            {
                return NotFound();
            }

            var vote = _repo.GetVote(postId, voteId);

            if (vote == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<VoteReadDto>(vote));
        }

        [HttpPost]
        public ActionResult<VoteReadDto> CreateVoteForPost(ObjectId postId, VoteCreateDto voteDto)
        {
            Console.WriteLine($"--> Hit CreateVoteForPost: {postId}");

            if (!_repo.PostExists(postId))
            {
                return NotFound();
            }

            var vote = _mapper.Map<Vote>(voteDto);

            _repo.CreateVote(postId, vote);
            _repo.SaveChanges();

            var voteReadDto = _mapper.Map<VoteReadDto>(vote);

            return Ok(voteReadDto);
        }
    }
}
