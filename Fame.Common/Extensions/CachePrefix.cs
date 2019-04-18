namespace Fame.Common.Extensions
{
    public static class CachePrefix
    {
        private static readonly string CurationKey = "curation__";

        //private static readonly string CurationMediaKey = "curation_media__";

        public static string Curation => $"{CurationKey}";

        //public static string CurationMedia => $"{CurationKey}{CurationMediaKey}";
    }

    public class CacheKey
    {
        private object[] _keyParts;

        private CacheKey(params object[] keyparts)
        {
            _keyParts = keyparts;
        }

        public string Key => string.Join("-", _keyParts);

        public static CacheKey Create(params object[] keyparts) 
        {
            return new CacheKey(keyparts);
        }
    }
}
