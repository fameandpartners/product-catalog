using System;

namespace Fame.Service.Services
{

    public partial class ImportService
    {
        private class GroupImport : IDataImport<GroupImport>
        {
            public string GroupName { get; set; }
            public string Title { get; set; }
            public int Order { get; set; }
            public string Slug { get; set; }
            public string SelectionTitle { get; set; }
            public bool Hidden { get;set; }

            public GroupImport FromCsv(string csvLine)
            {
                var values = csvLine.Split(',');
                return new GroupImport
                {
                    GroupName = values[0],
                    Title = values[1],
                    Order = Convert.ToInt16(values[2]),
                    Slug = values[3],
                    SelectionTitle = values[4],
                    Hidden = Boolean.TryParse(values[5], out var result) ? result : false
                };
            }

            public string TabTitle => "Group";
        }
    }
}