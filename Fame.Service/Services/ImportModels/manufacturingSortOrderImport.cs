using System;

namespace Fame.Service.Services
{

    public partial class ImportService
    {
        private class ManufacturingSortOrderImport : IDataImport<ManufacturingSortOrderImport>
        {
            public string Id { get; set; }
            public int Order { get; set; }

            public ManufacturingSortOrderImport FromCsv(string csvLine)
            {
                var values = csvLine.Split(',');
                return new ManufacturingSortOrderImport
                {
                    Id = values[0],
                    Order = Convert.ToInt16(values[1])
                };
            }

            public string TabTitle => "ManufacturingSortOrder";
        }
    }
}