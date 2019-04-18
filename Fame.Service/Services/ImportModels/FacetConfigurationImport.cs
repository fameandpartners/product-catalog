using System.Collections.Generic;

namespace Fame.Service.Services
{
    public partial class ImportService
    {
        private class FacetConfigurationImport : IDataImport<FacetConfigurationImport>
        {
            public string FacetConfigurationId { get; set; }
            public string Title { get; set; }
            public IEnumerable<string> FacetCategoryIds { get; set; }

            public FacetConfigurationImport FromCsv(string csvLine)
            {
                var values = csvLine.Split(',');
                return new FacetConfigurationImport
                {
                    FacetConfigurationId = values[0],
                    Title = values[1],
                    FacetCategoryIds = values[2].Split("|")
                };
            }

            public string TabTitle => "FacetConfiguration";
        }
    }
}