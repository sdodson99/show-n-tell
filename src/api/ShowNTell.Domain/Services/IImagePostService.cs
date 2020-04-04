using ShowNTell.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ShowNTell.Domain.Services
{
    public interface IImagePostService
    {
        Task<ImagePost> GetById(int id);
        Task<ImagePost> Create(ImagePost imagePost);
        Task<ImagePost> Update(int id, string description);
        Task<ImagePost> Update(int id, string description, IEnumerable<Tag> tags);
        Task<bool> Delete(int id);
        Task<bool> IsAuthor(int id, string email);
    }
}
