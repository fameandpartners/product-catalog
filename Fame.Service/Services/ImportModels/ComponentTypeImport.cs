using System;
using Fame.Data.Models;

namespace Fame.Service.Services
{
    public partial class ImportService
    {
        private class ComponentTypeImport : IDataImport<ComponentTypeImport>
        {
            public string ComponentTypeId { get; set; }
            public string Title { get; set; }
            public string SelectionTitle { get; set; }
            public string ParentComponentTypeId { get; set; }
            public bool IsProductCode { get; set; }
            public ComponentTypeCategory ComponentTypeCategory { get; set; }
            public SelectionType SelectionType { get; set; }
            public decimal SortWeigthDefault { get; set; }
            public decimal SortWeigthOther { get; set; }
            public bool AggregateOnIndex { get; set; }

            public ComponentTypeImport FromCsv(string csvLine)
            {
                var values = csvLine.Split(',');
                return new ComponentTypeImport
                {
                    ComponentTypeId = values[0],
                    IsProductCode = Convert.ToBoolean(values[1]),
                    ParentComponentTypeId = string.IsNullOrWhiteSpace(values[2]) ? null : values[2],
                    ComponentTypeCategory = (ComponentTypeCategory)Enum.Parse(typeof(ComponentTypeCategory), values[3]),
                    SelectionTitle = values[4],
                    Title = values[5],
                    SelectionType = (SelectionType)Enum.Parse(typeof(SelectionType), values[6]),
                    SortWeigthDefault = Convert.ToDecimal(values[7]),
                    SortWeigthOther = Convert.ToDecimal(values[8]),
                    AggregateOnIndex = Convert.ToBoolean(values[9]),
                };
            }

            public string TabTitle => "ComponentType";
        }
    }
}