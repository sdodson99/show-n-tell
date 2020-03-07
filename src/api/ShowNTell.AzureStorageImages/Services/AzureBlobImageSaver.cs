using ShowNTell.Domain.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ShowNTell.AzureStorageImages.Services
{
    public class AzureBlobImageSaver : IImageSaver
    {
        public async Task<string> SaveImage(FileStream imageStream)
        {
            throw new NotImplementedException();
        }
    }
}
