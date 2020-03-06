using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.FileProviders;
using System.IO;

namespace ShowNTell.Prototype.Extensions
{
    public static class UseClientExtension
    {
        /// <summary>
        /// Serve a static website from a directory.
        /// </summary>
        /// <param name="rootFolderName">The directory with the static website.</param>
        public static IApplicationBuilder UseClient(this IApplicationBuilder app, string rootFolderName)
        {
            IFileProvider fileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), rootFolderName));

            return app
                .UseDefaultFiles(new DefaultFilesOptions()
                {
                    FileProvider = fileProvider
                })
                .UseStaticFiles(new StaticFileOptions
                {
                    FileProvider = fileProvider
                });
        }
    }
}
