using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ShowNTell.Domain.Services
{
    public interface IImageSaver
    {
        Task<string> SaveImage(FileStream imageStream);
    }
}
