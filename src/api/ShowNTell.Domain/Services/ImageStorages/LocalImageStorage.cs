using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ShowNTell.Domain.Services.ImageStorages
{
    public class LocalImageStorage : IImageStorage
    {
        private readonly string _localImageDirectory;
        private readonly string _localImageBaseUri;

        public LocalImageStorage(string localImageDirectory, string localImageBaseUri)
        {
            _localImageDirectory = localImageDirectory;
            _localImageBaseUri = localImageBaseUri;
        }

        public async Task<string> SaveImage(Stream imageStream, string fileExtension)
        {
            string imageFileName = Guid.NewGuid().ToString() + fileExtension;

            await WriteImageToOutput(imageStream, imageFileName);

            string resourceLocation = Path.Combine(_localImageBaseUri, imageFileName);

            return resourceLocation;
        }

        public async Task<bool> DeleteImage(string fileUri)
        {
            bool success = false;

            string imageFileName = fileUri.Substring(_localImageBaseUri.Length).Trim('/', '\\');
            string localImageLocation = Path.Combine(_localImageDirectory, imageFileName);

            try
            {
                if(File.Exists(localImageLocation))
                {
                    File.Delete(localImageLocation);
                    success = true;
                }
            }
            catch (Exception)
            {
                success = false;
            }

            return await Task.FromResult(success);

        }

        private async Task WriteImageToOutput(Stream imageStream, string imageId)
        {
            string outputPath = Path.Combine(_localImageDirectory, imageId);

            using (FileStream outputStream = new FileStream(outputPath, FileMode.CreateNew))
            {
                await imageStream.CopyToAsync(outputStream);
            }
        }
    }
}
