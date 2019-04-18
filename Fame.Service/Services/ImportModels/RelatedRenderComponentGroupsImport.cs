using System;
using System.Collections.Generic;
using System.Linq;

namespace Fame.Service.Services
{
    public partial class ImportService
    {
        private class RelatedRenderComponentGroupsImport : IDataImport<RelatedRenderComponentGroupsImport>
        {
            public string RenderTypeId { get; set; }
            public List<string> RelatedRenderComponents { get; set; }
            public string RenderPosition { get; set; }

            public RelatedRenderComponentGroupsImport FromCsv(string csvLine)
            {
                var values = csvLine.Split(',');
                return new RelatedRenderComponentGroupsImport
                {
                    RenderTypeId = values[0],
                    RelatedRenderComponents = values[1].Split("|", StringSplitOptions.RemoveEmptyEntries).ToList(),
                    RenderPosition = values[2]
                };
            }

            public string TabTitle => "RelatedRenderComponentGroup";
        }
    }
}
