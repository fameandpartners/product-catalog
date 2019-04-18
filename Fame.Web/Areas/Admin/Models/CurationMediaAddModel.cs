using Fame.Data.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Fame.Web.Areas.Admin.Models
{
	public class CurationEditModel
    {
        [Required]
        public string PID { get; set; } // Readonly

		public string Name { get; set; }

        public string Description { get; set; }

        [Display(Name = "Overlay Text")]
        [MaxLength(60, ErrorMessage = "Overlay Text must be less than 60 characters")]
        public string OverlayText { get; set; }

        public int? MediaAdded { get; internal set; }

		public Product Product { get; set; }

        public List<CurationComponent> CurationComponents { get; set; }

		public Facet Facet { get; set; }
		
		public string OnBodyUrlDomain { get; set; }

        public List<CurationMediaEditModel> CuratedMedia { get; set; }
	}

    public class CurationMediaEditModel
    {
        [Required]
        public int Id { get; set; } // Readonly
        
        public List<CurationMediaVariant> Variants { get; set; } // Readonly

        [Display(Name = "PDP Sort Order")]
        public int SortOrder { get; set; }

        [Display(Name = "PLP Sort Order")]
        public int PLPSortOrder { get; set; }

        [Display(Name = "Fit Description")]
        public string FitDescription { get; set; }
		
		[Display(Name = "Size Description")]
        public string SizeDescription { get; set; }
    }
}
