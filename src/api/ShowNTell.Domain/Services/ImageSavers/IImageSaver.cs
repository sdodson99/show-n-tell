using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ShowNTell.Domain.Services.ImageSavers
{
    public interface IImageSaver
    {
        Task<string> SaveImage(Stream imageStream);
    }
}
