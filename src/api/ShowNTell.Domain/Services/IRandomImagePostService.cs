using ShowNTell.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ShowNTell.Domain.Services
{
    public interface IRandomImagePostService
    {
        Task<ImagePost> GetRandom();
    }
}
