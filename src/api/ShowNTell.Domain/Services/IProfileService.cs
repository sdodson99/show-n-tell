using ShowNTell.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ShowNTell.Domain.Services
{
    public interface IProfileService
    {
        Task<User> GetProfile(string username);
        Task<IEnumerable<ImagePost>> GetImagePosts(string username);
    }
}
