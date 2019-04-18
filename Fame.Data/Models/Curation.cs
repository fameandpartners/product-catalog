using System;
using System.Collections.Generic;

namespace Fame.Data.Models
{
    [Serializable]
    public class Curation : ISearchMeta
    {
        public string PID { get; set; } // PID (may include extras)
        public string ProductId { get; set; }
        public string ProductDocumentVersionId { get; set; } // PID -extras
        public string PrimarySilhouetteId { get; set; } // FacetId of the PrimarySilhouette (Used for grouping and searching)
		public string Name { get; set; }
		public string Description { get; set; }
        public string OverlayText { get; set; }

        public Product Product { get; set; }
        public List<CurationComponent> CurationComponents { get; set; }
        public List<CurationMedia> Media { get ; set; }

        public Facet Facet { get; set; }

        public string TaxonString { get; set; }

        public string[] Taxons => string.IsNullOrEmpty(TaxonString) ? new string[] { } : TaxonString.Split("|"); 
    }

	public interface ISearchMeta
	{
		string ProductDocumentVersionId { get; set; }
		string PrimarySilhouetteId { get; set; }
		string Name { get; set; }
        string TaxonString { get; set; }
        string Description { get; set; }
	}
}
