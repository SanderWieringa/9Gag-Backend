using AutoMapper;
using Microsoft.Extensions.Options;
using VoteService.Dtos;
using VoteService.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace VoteService.Profiles
{
    public class VoteProfile : Profile
    {
        public VoteProfile()
        {
            CreateMap<Post, PostReadDto>();
            CreateMap<VoteCreateDto, Vote>();
            CreateMap<Vote, VoteReadDto>();
            CreateMap<PostPublishedDto, Post>()
                .ForMember(destination => destination.ExternalId, options => options.MapFrom(source => source.Id));
        }
    }
}
