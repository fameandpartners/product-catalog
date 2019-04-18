using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Fame.Data.Models;
using Fame.Service.Extensions;

namespace Fame.Service.DTO
{
    [Serializable]
    public class FileMeta
    {
        public string FullPath { get; set; }
        public DateTime LastModified { get; set; }

        public string FileName
        {
            get
            {
                return Path.GetFileName(FullPath);
            }
        }

        public static string GetFolder(string groupId, Orientation orientation, Zoom? zoom, Size size)
        {
            return Path.Combine(
                groupId,
                "layers",
                orientation.ToString(),
                zoom?.ToString() ?? "-",
                size.ToString()
            );
        }

        public static string GetLayerBaseFolder(string groupId, Orientation orientation)
        {
            return Path.Combine(
                groupId,
                "layers",
                orientation.ToString()
            );
        }

        public static FileMeta GetForResizedLayer(string groupId, Orientation orientation, Zoom? zoom, FileMeta layer, Size size)
        {
            return new FileMeta()
            {
                LastModified = layer.LastModified,
                FullPath = $"{GetFolder(groupId, orientation, zoom, size)}/{layer.FileName}"
            };
        }
        
        public static FileMeta GetForRenderedImage(string groupId, Zoom zoom, Orientation orientation, Size size, string fileName, IEnumerable<FileMeta> files)
        {
            var lastModified = files.OrderByDescending(x => x.LastModified).FirstOrDefault()?.LastModified ?? new DateTime(1990, 1, 1);
            return GetForRenderedImage(groupId, zoom, orientation, size, fileName, lastModified);
        }

        public static FileMeta GetForOnBodyImage(CurationMediaVariant curationMediaVariant)
        {
            return curationMediaVariant.ToFileMeta();
        }

        public static FileMeta GetForDocument(FeedMeta feedMeta)
        {
            return new FileMeta()
            {
                FullPath = feedMeta.S3Path,
                LastModified = feedMeta.CreatedOn
            };
        }

        public static FileMeta GetForRenderedImage(string groupId, Zoom zoom, Orientation orientation, Size size, string fileName, DateTime lastModified)
        {
            var path = Path.Combine(
                groupId,
                $"{orientation.ToString()}{zoom.ToString()}",
                size.ToString(),
                fileName
            );

            return new FileMeta()
            {
                LastModified = lastModified,
                FullPath = path
            };
        }

        public static string GetFileName(IEnumerable<string> options, string extension)
        {
            var sortedOptions = options.OrderBy(x => x);
            return $"{String.Join("~", sortedOptions)}.{extension}";
        }
    }
}
