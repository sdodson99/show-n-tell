using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ShowNTell.Domain.Services.ImageStorages
{
    public interface IImageStorage
    {
        Task<string> SaveImage(Stream imageStream, string fileExtension);
        Task<bool> DeleteImage(string fileUri);
    }
}
