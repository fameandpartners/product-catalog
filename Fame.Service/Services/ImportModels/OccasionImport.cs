using System;
using System.Collections.Generic;
using System.Linq;

namespace Fame.Service.Services
{
    public partial class ImportService
    {
        private class OccasionImport : IDataImport<OccasionImport>
        {
            public string OccasionId { get; set; }
            public string OccasionName { get; set; }
            public string ComponentCompatibilityRule { get; set; }
            public string FacetCompatibilityRule { get; set; }
            public List<string> CollectionIds { get; set; }

            public OccasionImport FromCsv(string csvLine)
            {
                var values = csvLine.Split(',');
                return new OccasionImport
                {
                    OccasionId = values[0],
                    OccasionName = values[1],
                    ComponentCompatibilityRule = values[2],
                    FacetCompatibilityRule = values[3],
                    CollectionIds = values[4].Split("|", StringSplitOptions.RemoveEmptyEntries).ToList()
                };
            }

            public string TabTitle => "Occasion";
        }
    }
}