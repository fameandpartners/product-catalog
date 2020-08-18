using System;
using System.Collections.Generic;
using System.Linq;

namespace Fame.Service.Services
{
    public partial class ImportService
    {
        private class FacetImport : IDataImport<FacetImport>
        {
            public string FacetId { get; set; }
            public string Title { get; set; }
            public string Subtitle { get; set; }
            public string FacetGroupId { get; set; }
            public string CompatibilityRule { get; set; }
            public int Order { get; set; }
            public string PreviewImage { get; set; }
            public IEnumerable<KeyValuePair<string, string>> FacetMeta { get; set; }
            public int TagPriority { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public List<string> CollectionIds { get; set; }
            public string TaxonString { get; set; }

            public FacetImport FromCsv(string csvLine)
            {
                var values = csvLine.Split(',');
                var ret = new FacetImport
                {
                    FacetId = values[0],
                    Title = values[1],
                    Subtitle = values[2],
                    FacetGroupId = values[3],
                    CompatibilityRule = values[4],
                    Order = int.Parse(values[5]),
                    PreviewImage = values[6],
                    FacetMeta = values[7].Split("|", StringSplitOptions.RemoveEmptyEntries).Select(c => Util.CreateKeyValue(c.Split("=", StringSplitOptions.RemoveEmptyEntries))),
                    TagPriority = int.TryParse(values[8], out var productNameOrder) ? productNameOrder : 0,
                    Name = values[9],
                    Description = values[10].Replace("^", ","),
                    CollectionIds = values[11].Split("|", StringSplitOptions.RemoveEmptyEntries).ToList(),
                    TaxonString = values[12]
                };
                Console.WriteLine("facet import:");
                Console.WriteLine(ret.FacetId);
                Console.WriteLine(ret.Title);
                Console.WriteLine(ret.Subtitle);
                Console.WriteLine(ret.FacetGroupId);
                Console.WriteLine(ret.CompatibilityRule);
                Console.WriteLine(ret.Name);
                Console.WriteLine(ret.Description);
                Console.WriteLine(ret.TaxonString);
                return ret;
            }
            
            public string TabTitle => "Facet";
        }
    }
}
