using ShowNTell.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ShowNTell.Domain.Services
{
    public interface IFollowService
    {
        Task<Follow> FollowUser(string userEmail, string followerEmail);
        Task<bool> UnfollowUser(string userEmail, string followerEmail);
    }
}
