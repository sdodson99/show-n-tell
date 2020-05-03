using System.IO;
using ExifOrient.AspNetCore;
using Microsoft.AspNetCore.Http;

namespace ShowNTell.API.Services.ImageOptimizations
{
    public class ExifImageOptimizationService : IImageOptimizationService
    {
        private readonly ExifImageFormFileOrientationNormalizer _orientationNormalizer;

        public ExifImageOptimizationService()
        {
            _orientationNormalizer = new ExifImageFormFileOrientationNormalizer();
        }

        public Stream Optimize(IFormFile image)
        {
            return _orientationNormalizer.Normalize(image);
        }
    }
}