using AutoMapper;
using ShowNTell.API.Models.Responses;
using ShowNTell.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShowNTell.API.Models.MappingProfiles
{
    public class ResponseDomainMappingProfile : Profile
    {
        public ResponseDomainMappingProfile()
        {
            CreateMap<Like, LikeResponse>();
            CreateMap<Comment, CommentResponse>()
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.User.Username));
            CreateMap<User, UserResponse>();
            CreateMap<ImagePost, ImagePostResponse>();
        }
    }
}
