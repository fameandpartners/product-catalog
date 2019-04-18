using System;
using System.Collections.Generic;

namespace Fame.Search.DTO
{
    [Serializable]
    public class FacetCount
    {
        public string FacetId { get; set; }

        public string Title { get; set; }

        public string Subtitle { get; set; }

        public int Order { get; set; }

        public long DocCount { get; set; }

        public string PreviewImage { get; set; }

        public Dictionary<string, string> FacetMeta { get; set; }

        public FacetCount SetPositiveDocCount()
        {
            DocCount = 1;
            return this;
        }
    }
}
