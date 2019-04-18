using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Fame.Data.Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Processing.Drawing;
using SixLabors.ImageSharp.Processing.Transforms;

namespace Fame.Service.Services
{
    public class ImageSharpImageManipulatorService : IImageManipulatorService
    {
        public async Task<IDictionary<Size, Stream>> LayerAndCrop(IEnumerable<Stream> files, IEnumerable<Size> sizes)
        {
            var imageInfo = Image.Identify(files.First());
            Size canvasSize = new Size(imageInfo.Width, imageInfo.Height, 0);

            using (var layeredImage = new Image<Rgba32>(canvasSize.Width, canvasSize.Height))
            {
                layeredImage.Mutate(x =>
                {
                    foreach (var layer in files)
                    {
                        using (Image<Rgba32> file = Image.Load(layer))
                        {
                            x.DrawImage(file, 1.0f);
                        }
                        layer.Dispose();
                    }

                });


                var result = sizes.ToDictionary(
                    size => size,
                    size =>
                    {
                        Image<Rgba32> resizedImage = null;

                        var newSize = Size.CalculateDimensions(new Size(canvasSize.Width, canvasSize.Height, 0), new Size.MaxSize() { ShortSide = size.Height, LongSide = size.Width });
                        if (newSize.HasValue)
                        {
                            resizedImage = layeredImage.Clone(x =>
                            {
                                x.Resize(newSize.Value.Width, newSize.Value.Height);
                            });
                        }
                        else
                        {
                            resizedImage = layeredImage;
                        }

                        var ms = new MemoryStream();
                        resizedImage.SaveAsPng(ms, new PngEncoder()
                        {
                            CompressionLevel = 9,
                            PngColorType = PngColorType.Palette
                        });
                        ms.Position = 0;


                        if (resizedImage != layeredImage)
                        {
                            resizedImage.Dispose();
                        }

                        return ms as Stream;
                    }
                );



                return result;
            }

        }

        public async Task<Stream> ResizeAndCrop(Stream file, Size size, Orientation orientation, Zoom zoom)
        {
            var image = Image.Load(file);

            var cropRectangle = zoom.GetCroppingRectangle(orientation, image.Width, image.Height);
            var newSize = Size.CalculateDimensions(new Size(image.Width, image.Height, 0), new Size.MaxSize() { ShortSide = size.Height, LongSide = size.Width });

            image.Mutate(x =>
            {
                x.Crop(new SixLabors.Primitives.Rectangle(cropRectangle.X, cropRectangle.Y, cropRectangle.Width, cropRectangle.Height));

                if (newSize.HasValue)
                {
                    x.Resize(newSize.Value.Width, newSize.Value.Height);
                }
            });

            var ms = new MemoryStream();
            image.SaveAsPng(ms);
            ms.Position = 0;
            return ms;
        }

        public async Task<Stream> ResizeToJpeg(Stream file, Size size)
        {
            using (var image = Image.Load(file))
            {
                image.Mutate(x => { x.Resize(size.Width, size.Height); });
                var ms = new MemoryStream();
                image.SaveAsJpeg(ms, new JpegEncoder() { Quality = 90 });
                ms.Position = 0;
                return ms;
            };
        }
    }
}
