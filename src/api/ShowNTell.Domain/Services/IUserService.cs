using ShowNTell.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ShowNTell.Domain.Services
{
    public interface IUserService
    {
        Task<User> GetById(int id);
        Task<User> GetByEmail(string email);
        Task<User> Create(User user);
        Task<User> Update(int id, User user);
        Task<bool> Delete(int id);
    }
}
