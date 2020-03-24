using ShowNTell.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ShowNTell.Domain.Services
{
    public interface IFollowService
    {
        Task<Follow> FollowUser(string username, string followerEmail);
        Task<bool> UnfollowUser(string username, string followerEmail);
    }
}
