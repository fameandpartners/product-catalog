using System;

namespace Fame.Service.Services
{
    public partial class ImportService
    {
        private class FacetGroupImport : IDataImport<FacetGroupImport>
        {
            public string FacetGroupId { get; set; }
            public string Title { get; set; }
            public string Subtitle { get; set; }
            public int Sort { get; set; }
            public bool IsAggregatedFacet { get; set; }
            public bool IsCategoryFacet { get; set; }
            public bool Multiselect { get; set; }
            public bool Collapsed { get; set; }
            public string Slug { get; set; }
            public string Name { get; set; }
            public int? ProductNameOrder { get; set; }
            public bool PrimarySilhouette { get; set; }

            public FacetGroupImport FromCsv(string csvLine)
            {
                var values = csvLine.Split(',');
                return new FacetGroupImport
                {
                    FacetGroupId = values[0],
                    Title = values[1],
                    Subtitle = values[2],
                    Sort = int.Parse(values[3]),
                    IsAggregatedFacet = Convert.ToBoolean(values[4]),
                    Multiselect = Convert.ToBoolean(values[5]),
                    Collapsed = Convert.ToBoolean(values[6]),
                    IsCategoryFacet = Convert.ToBoolean(values[7]),
                    Slug = values[8],
                    Name = values[9],
                    ProductNameOrder = int.TryParse(values[10], out var productNameOrder) && productNameOrder > 0 ? productNameOrder : (int?)null,
                    PrimarySilhouette = Convert.ToBoolean(values[11])
                };
            }

            public string TabTitle => "FacetGroup";
        }
    }
}