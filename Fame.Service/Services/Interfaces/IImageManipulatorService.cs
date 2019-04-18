using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Fame.Data.Models;
using Fame.Service;

namespace Fame.Service.Services
{
    public interface IImageManipulatorService
    {
        Task<Stream> ResizeAndCrop(Stream file, Size size, Orientation orientation, Zoom zoom);
        Task<Stream> ResizeToJpeg(Stream file, Size size);
        Task<IDictionary<Size, Stream>> LayerAndCrop(IEnumerable<Stream> files, IEnumerable<Size> size);
    }
}
