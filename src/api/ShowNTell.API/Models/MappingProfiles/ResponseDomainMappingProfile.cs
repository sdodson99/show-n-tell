using AutoMapper;
using ShowNTell.API.Models.Responses;
using ShowNTell.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShowNTell.API.Models.MappingProfiles
{
    /// <summary>
    /// A profile for mapping domain objects to response objects.
    /// </summary>
    public class ResponseDomainMappingProfile : Profile
    {
        public ResponseDomainMappingProfile()
        {
            CreateMap<Tag, TagResponse>();
            CreateMap<Like, LikeResponse>();
            CreateMap<Comment, CommentResponse>()
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.User.Username));
            CreateMap<Follow, FollowResponse>()
                .ForMember(dest => dest.UserUsername, opt => opt.MapFrom(src => src.User.Username))
                .ForMember(dest => dest.FollowerUsername, opt => opt.MapFrom(src => src.Follower.Username));
            CreateMap<User, ProfileResponse>();
            CreateMap<User, UserResponse>();
            CreateMap<ImagePost, ImagePostResponse>()
                .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags.Select(t => t.Tag).ToList()));
        }
    }
}
