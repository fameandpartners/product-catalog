using System;
using System.Collections.Generic;
using System.Linq;

namespace Fame.Service.Services
{

    public partial class ImportService
    {
        private class IncompatibilityImport : IDataImport<IncompatibilityImport>
        {
            public string ProductId { get; set; }
            public string ParentComponentId { get; set; }
            public string ComponentId { get; set; }
            public List<List<string>> IncompatibleWith { get; set; }

            public IncompatibilityImport FromCsv(string csvLine)
            {
                IncompatibilityImport incompatibilityImport = null;
                try
                {
                    var values = csvLine.Split(',');
                    incompatibilityImport = new IncompatibilityImport
                    {
                        ProductId = values[0],
                        ParentComponentId = values[1] ?? "allOptions",
                        ComponentId = values[2],
                        IncompatibleWith = values[3].Split("|", StringSplitOptions.RemoveEmptyEntries).ToList().Select(fc => fc.Split("&", StringSplitOptions.RemoveEmptyEntries).ToList()).ToList(),
                    };
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                return incompatibilityImport;
            }

            public string TabTitle => "Incompatibility";
        }
    }
}