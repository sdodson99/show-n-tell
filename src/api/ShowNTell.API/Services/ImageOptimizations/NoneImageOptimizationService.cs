using System.IO;
using Microsoft.AspNetCore.Http;

namespace ShowNTell.API.Services.ImageOptimizations
{
    public class NoneImageOptimizationService : IImageOptimizationService
    {
        public Stream Optimize(IFormFile image)
        {
            return image.OpenReadStream();
        }
    }
}