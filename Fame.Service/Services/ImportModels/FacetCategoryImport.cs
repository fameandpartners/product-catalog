using System.Collections.Generic;

namespace Fame.Service.Services
{
    public partial class ImportService
    {
        private class FacetCategoryImport : IDataImport<FacetCategoryImport>
        {
            public string FacetCategoryId { get; set; }
            public string Title { get; set; }
            public IEnumerable<string> FacetGroupIds { get; set; }
            public int Sort { get; set; }
            public bool HideHeader { get; set; }

            public FacetCategoryImport FromCsv(string csvLine)
            {
                var values = csvLine.Split(',');
                return new FacetCategoryImport
                {
                    FacetCategoryId = values[0],
                    Title = values[1],
                    FacetGroupIds = values[2].Split("|"),
                    Sort = int.Parse(values[3]),
                    HideHeader = bool.Parse(values[4])
                };
            }

            public string TabTitle => "FacetCategory";
        }
    }
}