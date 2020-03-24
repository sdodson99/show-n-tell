﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShowNTell.API.Models.Responses
{
    public class ProfileResponse
    {
        public string Email { get; set; }
        public string Username { get; set; }
        public DateTime DateJoined { get; set; }

        public ICollection<ImagePostResponse> ImagePosts { get; set; }
        public ICollection<FollowResponse> Followers { get; set; }
        public ICollection<FollowResponse> Following { get; set; }
    }
}