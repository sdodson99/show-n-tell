using ShowNTell.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ShowNTell.Domain.Services
{
    public interface IImagePostService
    {
        Task<IEnumerable<ImagePost>> GetAllByUsername(string username);
        Task<IEnumerable<ImagePost>> GetAllByTagId(int id);
        Task<ImagePost> GetById(int id);
        Task<ImagePost> Create(ImagePost imagePost);
        Task<ImagePost> Update(int id, ImagePost imagePost);
        Task<ImagePost> UpdateDescription(int id, string description);
        Task<bool> Delete(int id);
    }
}
