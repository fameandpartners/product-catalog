using System.Collections.Generic;

namespace Fame.Service.DTO
{
    public class FacetSummary
    {
        public string FacetId { get; set; }

        public string Title { get; set; }

        public string Subtitle { get; set; }

        public int Order { get; set; }

        public string PreviewImage { get; set; }

        public Dictionary<string, string> FacetMeta { get; set; }

        public bool Selected { get; set; }
    }
}