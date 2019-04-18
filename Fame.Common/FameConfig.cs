using System.Linq;

namespace Fame.Common
{
    public class FameConfig
    {
        public ElasticConfig Elastic { get; set; }
        public SpreeConfig Spree { get; set; }
        public SizeConfig Size { get; set; }
        public LocalisationConfig Localisation { get; set; }
        public ImportConfig Import { get; set; }
        public ImageProcessingConfig ImageProcessing { get; set; }
        public DropboxConfig Dropbox { get; set; }
        public RenderConfig Render { get; set; }
        public CurationsConfig Curations { get; set; }
        public DocumentConfig Document { get; set; }
        public CacheConfig Cache { get; set; }
    }

    public class RenderConfig
    {
        public string Url { get; set; }
        public string DefaultColor { get; set; }
    }

    public class CacheConfig
    {
        public string Server { get; set; }
        public string Port { get; set; }
        public string InstanceName { get; set; }
    }

    public class ElasticConfig
    {
        public string ConnectionString { get; set; }
        public string SearchIndexName { get; set; }
        public int SearchPageSize { get; set; }
    }

    public class SpreeConfig
    {
        public string BaseUrl { get; set; }
    }

    public class SizeConfig
    {
        public int MinHeightCm { get; set; }
        public int MaxHeightCm { get; set; }
        public int MinHeightInch { get; set; }
        public int MaxHeightInch { get; set; }
    }

    public class LocalisationConfig
    {
        public string US { get; set; }
        public string AU { get; set; }

        public bool IsValidLocaleCode(string code)
        {
            return GetType().GetProperties().Select(prop => prop.GetValue(this) as string).Any(value => value == code);
        }
    }

    public class ImportConfig
    {
        public string Url { get; set; }
    }

    public class ImageProcessingConfig
    {
        public string S3BucketName { get; set; }
        public string LayeringQueue { get; set; }
        public string FileSyncQueue { get; set; }
    }

    public class CurationsConfig
    {
        public string S3BucketName { get; set; }
        public string Url { get; set; }
        public string DropboxImportPath { get; set; }
    }

    public class DocumentConfig
    {
        public string S3BucketName { get; set; }
        public string Url { get; set; }
    }

    public class DropboxConfig
    {
        public string AccessKey { get; set; }
    }
}
