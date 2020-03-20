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
            CreateMap<Like, LikeResponse>().ReverseMap();
            CreateMap<Comment, CommentResponse>().ReverseMap();
            CreateMap<User, UserResponse>().ReverseMap();
            CreateMap<ImagePost, ImagePostResponse>().ReverseMap();
        }
    }
}
