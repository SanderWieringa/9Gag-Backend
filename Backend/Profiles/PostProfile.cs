using AutoMapper;
using PostService.Dtos;
using PostService.Models;

namespace PostService.Profiles
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
