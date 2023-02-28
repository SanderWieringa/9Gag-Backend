using AutoMapper;
using Backend.Dtos;
using Backend.Models;

namespace Backend.Profiles
{
    public class PostProfile : Profile
    {
        public PostProfile()
        {
            CreateMap<Post, PostReadDto>();
            CreateMap<PostCreateDto, Post>();
        }
    }
}
