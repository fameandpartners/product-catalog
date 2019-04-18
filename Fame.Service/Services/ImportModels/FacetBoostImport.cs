using System;
using System.Collections.Generic;
using System.Linq;

namespace Fame.Service.Services
{
    public partial class ImportService
    {
        private class FacetBoostImport : IDataImport<FacetBoostImport>
        {
            public string BoostRule { get; set; }
            public decimal BoostWeight { get; set; }
            public List<string> CollectionIds { get; set; }

            public FacetBoostImport FromCsv(string csvLine)
            {
                var values = csvLine.Split(',');
                return new FacetBoostImport
                {
                    BoostRule = values[0],
                    BoostWeight = decimal.Parse(values[1]),
                    CollectionIds = values[2].Split("|", StringSplitOptions.RemoveEmptyEntries).ToList()
                };
            }

            public string TabTitle => "FacetBoost";
        }
    }
}