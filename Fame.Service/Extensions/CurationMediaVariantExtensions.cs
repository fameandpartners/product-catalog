using Fame.Data.Models;
using Fame.Service.DTO;
using System;
using System.IO;

namespace Fame.Service.Extensions
{
    public static class CurationMediaVariantExtensions
    {
        public static Size ToSize(this CurationMediaVariant cmv)
        {
            return new Size(cmv.Width, cmv.Height, cmv.Quality, cmv.IsOriginal);
        }

        public static FileMeta ToFileMeta(this CurationMediaVariant cmv)
        {
            return new FileMeta
            {
                FullPath = cmv.ToS3Path(),
                LastModified = DateTime.UtcNow
            };
        }

        public static String ToS3Path(this CurationMediaVariant cmv)
        {
            var size = cmv.ToSize();
            var sizeString = size.IsOriginal ? "original" : size.ToString();
            return Path.Combine(
                    cmv.CurationMedia.PID,
                    cmv.CurationMediaId.ToString(),
                    $"{sizeString}{cmv.Ext}")
                    .Replace(@"\", @"/");
        }

        public static String ToFullPath(this CurationMediaVariant cmv, string curationsUrl)
        {
            var size = cmv.ToSize();
            var sizeString = size.IsOriginal ? "original" : size.ToString();
            return Path.Combine(
                    curationsUrl,
                    cmv.CurationMedia.PID,
                    cmv.CurationMediaId.ToString(),
                    $"{sizeString}{cmv.Ext}")
                    .Replace(@"\", @"/");
        }
    }
}
