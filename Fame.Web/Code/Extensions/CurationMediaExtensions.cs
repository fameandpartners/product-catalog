using Fame.Data.Models;
using Fame.Service.DTO;
using Fame.Web.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fame.Web.Code.Extensions
{
    public static class CurationMediaExtensions
    {
        public static List<CurationMediaEditModel> ToCurationMediaEditModel(this List<CurationMedia> curationMedia)
        {
            return curationMedia.Select(cm => new CurationMediaEditModel
            {
                Id = cm.Id, 
                FitDescription = cm.FitDescription, 
                SizeDescription = cm.SizeDescription,
                SortOrder = cm.SortOrder,
                PLPSortOrder = cm.PLPSortOrder,
                Variants = cm.CurationMediaVariants
            })
            .ToList();
        }

        public static List<CurationMedia> ToCurationMedia(this List<CurationMediaEditModel> curationMedia)
        {
            return curationMedia.Select(cm => new CurationMedia
            {
                Id = cm.Id, 
                FitDescription = cm.FitDescription, 
                SizeDescription = cm.SizeDescription, 
                SortOrder = cm.SortOrder,
                PLPSortOrder = cm.PLPSortOrder,
            })
            .ToList();
        }
    }
}
