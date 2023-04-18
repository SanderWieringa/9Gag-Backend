using AutoMapper;
using PostService.Dtos;
using PostService.Models;
using PostService.Dtos;

namespace PostService.Profiles
{
    public class PostProfile : Profile
    {
        public PostProfile()
        {
            CreateMap<Post, PostReadDto>();
            CreateMap<PostCreateDto, Post>();
            CreateMap<PostReadDto, PostPublishedDto>();
        }
    }
}
