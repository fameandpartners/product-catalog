using System;
using System.Collections.Generic;

namespace Fame.Service.Services
{
    public partial class ImportService
    {
        private class SectionGroupImport : IDataImport<SectionGroupImport>
        {
            public string SectionGroupName { get; set; }
            public string Title { get; set; }
            public string GroupName { get; set; }
            public IEnumerable<string> SectionComponentTypeIds { get; set; }
            public string RenderPosition { get; set; }
            public int Order { get; set; }
            public string Slug { get; set; }
            public string AggregateTitle { get; set; }

            public SectionGroupImport FromCsv(string csvLine)
            {
                var values = csvLine.Split(',');
                SectionGroupImport sectionGroupImport = null;
                try
                {
                    sectionGroupImport = new SectionGroupImport
                    {
                        SectionGroupName = values[0],
                        Title = values[1],
                        GroupName = values[2],
                        SectionComponentTypeIds = values[3].Split("|", StringSplitOptions.RemoveEmptyEntries),
                        RenderPosition = values[4],
                        Order = Convert.ToInt16(values[5]),
                        Slug = values[6],
                        AggregateTitle = values[7]
                    };
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return sectionGroupImport;
            }

            public string TabTitle => "SectionGroup";
        }
    }
}