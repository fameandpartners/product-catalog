using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Fame.Data.Models;
using Fame.Service.DTO;
using Microsoft.EntityFrameworkCore;

namespace Fame.Service.Extensions
{
    public static class CurationMediaQueryExtensions
    {
        public static Dictionary<string, ProductListItem> ToMediaListDictionary(this IQueryable<Curation> repo, string curationsUrl)
        {
            return repo
                .Include(c => c.Media)
                .ThenInclude(c => c.CurationMediaVariants)
                .ToList()
                .Select(c => new ProductListItem
                {
                    PID  = c.PID,
                    OverlayText = c.OverlayText,
                    TaxonString = c.TaxonString,
                    Media = c.Media.OrderBy(m => m.SortOrder).Select(m => new MediaListItem
                        {
                            PID = m.PID,
                            Type = m.Type,
                            SortOrder = m.SortOrder,
                            FitDescription = m.FitDescription,
                            SizeDescription = m.SizeDescription,
                            Src = m.CurationMediaVariants
                                    .OrderBy(cmv => cmv.Width)
                                    .Select(cmv => new MediaSummary
                                    {
                                        Height = cmv.Height,
                                        Name = cmv.Id.ToString(),
                                        Url = cmv.ToFullPath(curationsUrl),
                                        Width = cmv.Width
                                    })
                                    .ToList()
                        }).ToList()
                    } 
                )
                .ToDictionary(c => c.PID, c => c);
        }

        public static List<MediaListItem> ToMediaListItem(this IQueryable<CurationMedia> repo, string curationsUrl)
        {
            return repo
                .Include(c => c.Curation)
                .ThenInclude(c => c.CurationComponents)
                .Include(c => c.CurationMediaVariants)
                .OrderBy(m => m.SortOrder)
                .Select(c => new { 
                    CurationMedia = c,
                    c.Curation, 
                    Options  = c.Curation.CurationComponents.Where(cc => cc.Component.ComponentTypeId == "color").Select(ct => ct.ComponentId).ToList(),
                    c.CurationMediaVariants
                })
                .ToList()
                .Select(m => new MediaListItem
                    {
                        PID = m.CurationMedia.PID,
                        Type = m.CurationMedia.Type,
                        SortOrder = m.CurationMedia.SortOrder,
                        FitDescription = m.CurationMedia.FitDescription,
                        SizeDescription = m.CurationMedia.SizeDescription,
                        Src = m.CurationMediaVariants
                                    .OrderBy(cmv => cmv.Width)
                                    .Select(cmv => new MediaSummary
                                    {
                                        Height = cmv.Height,
                                        Name = cmv.Id.ToString(),
                                        Url = cmv.ToFullPath(curationsUrl),
                                        Width = cmv.Width
                                    })
                                    .ToList(),
                        Options = m.Options
                    })
                    .ToList();
        }
    }
}
