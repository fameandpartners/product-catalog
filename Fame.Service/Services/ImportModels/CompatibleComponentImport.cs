using System;
using System.Collections.Generic;
using System.Linq;

namespace Fame.Service.Services
{
    public partial class ImportService
    {
        private class CompatibleComponentImport : IDataImport<CompatibleComponentImport>
        {
            public string ProductId { get; set; }
            public string ParentComponentId { get; set; }
            public List<string> ComponentIds { get; set; }

            public CompatibleComponentImport FromCsv(string csvLine)
            {
                var values = csvLine.Split(',');
                return new CompatibleComponentImport
                {
                    ProductId = values[0],
                    ParentComponentId = values[1],
                    ComponentIds = values[2].Split("|", StringSplitOptions.RemoveEmptyEntries).ToList()
                };
            }

            public string TabTitle => "CompatibleComponent";
        }
    }
}