using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ShowNTell.Domain.Services.ImageSavers
{
    public class LocalImageSaver : IImageSaver
    {
        private readonly string _localImageDirectory;
        private readonly string _localImageBaseUri;

        public LocalImageSaver(string localImageDirectory, string localImageBaseUri)
        {
            _localImageDirectory = localImageDirectory;
            _localImageBaseUri = localImageBaseUri;
        }

        public async Task<string> SaveImage(Stream imageStream)
        {
            string imageFileName = Guid.NewGuid().ToString() + ".txt";

            await WriteImageToOutput(imageStream, imageFileName);

            string resourceLocation = Path.Combine(_localImageBaseUri, imageFileName);

            return resourceLocation;
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
