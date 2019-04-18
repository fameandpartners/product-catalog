using System;
using System.IO;

namespace Fame.Data.Models
{
    public class FeedMeta
    {
        public int Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public string Ext { get; set; }

        public bool Zipped { get; set; }

        public string FileName => $"{CreatedOn.Ticks}.{Ext}";

        public string ZipFileName => $"{CreatedOn.Ticks}.zip";

        public string S3Path => Path.Combine("Feed", Zipped ? ZipFileName : FileName).Replace(@"\", @"/");

        public string FullPath(string url) => Path.Combine(url, S3Path).Replace(@"\", @"/");

        public static FeedMeta Create(string ext, bool zipped = false)
        {
            return new FeedMeta() { CreatedOn = DateTime.UtcNow, Ext = ext, Zipped = zipped };
        }
    }
}
