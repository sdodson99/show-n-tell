using System.IO;
using Microsoft.AspNetCore.Http;

namespace ShowNTell.API.Services.ImageOptimizations
{
    public interface IImageOptimizationService
    {
        /// <summary>
        /// Optimize a form file image.
        /// </summary>
        /// <param name="image">The form file image to optimize.</param>
        /// <returns>The optimized form file image as a stream.</returns>
        Stream Optimize(IFormFile image);
    }
}