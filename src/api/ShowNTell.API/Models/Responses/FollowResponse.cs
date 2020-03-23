using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShowNTell.API.Models.Responses
{
    public class FollowResponse
    {
        public string UserEmail { get; set; }
        public string UserUsername { get; set; }

        public string FollowerEmail { get; set; }
        public string FollowerUsername { get; set; }
    }
}
