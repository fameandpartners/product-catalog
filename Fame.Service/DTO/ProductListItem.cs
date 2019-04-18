using Fame.Data.Models;
using System;
using System.Collections.Generic;

namespace Fame.Service.DTO
{
    [Serializable]
    public class ProductListItem
    {
        public string PID { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string OverlayText { get; set; }

        public string Url { get; set; }

        public string PreviewType => "render"; // All products in the product catalogue are currently renders

        public List<MediaListItem> Media { get; set; }

        public decimal Price { get; set; }

        public string PrimarySilhouetteId { get; internal set; }
        public string TaxonString { get; internal set; }
    }

    [Serializable]
    public class MediaListItem
    {
        public string PID { get; set; }

        public MediaType Type { get; set; }

        public List<MediaSummary> Src { get; set; }
        
        public int SortOrder { get; set; }

        public string FitDescription { get; set; }

        public string SizeDescription { get; set; }
        public List<string> Options { get; set; }
    }

    [Serializable]
    public class MediaSummary
    {
        public string Name { get; set; }
        
        public int Width { get; set; }
        
        public int Height { get; set; }

        public string Url { get; set; }
    }
}
