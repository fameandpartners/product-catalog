using Fame.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fame.Service.Services
{
    public partial class ImportService
    {
        private class ComponentImport : IDataImport<ComponentImport>
        {
            public string ComponentId { get; private set; }
            public string Title { get; private set; }
            public string ComponentTypeId { get; private set; }
            public string OptionRenderComponentType { get; private set; }
            public IEnumerable<string> DefaultOptionIn { get; private set; }
            public int AUD { get; set; }
            public int USD { get; set; }
            public string CartId { get; set; }
            public int SortOrder { get; set; }
            public IEnumerable<KeyValuePair<string, string>> ComponentMeta { get; set; }
            public bool Indexed { get; set; }
            public string ManufacturingSortOrderId { get; set; }
            public bool IsolateInSummary { get; set; }
            public Zoom? PreviewZoom { get; set; }

            public ComponentImport FromCsv(string csvLine)
            {
                var values = csvLine.Split(',');
                ComponentImport componentImport = null;
                try
                {
                    componentImport = new ComponentImport
                    {
                        ComponentId = values[0],
                        Title = values[1],
                        ComponentTypeId = values[2],
                        OptionRenderComponentType = values[3],
                        DefaultOptionIn = values[4].Split("|", StringSplitOptions.RemoveEmptyEntries),
                        AUD = int.TryParse(values[5], out var i) ? i * 100 : 0,
                        USD = int.TryParse(values[6], out i) ? i * 100 : 0,
                        CartId = string.IsNullOrWhiteSpace(values[7]) ? null : values[7],
                        SortOrder = Convert.ToInt32(values[8]),
                        ComponentMeta = values[9].Split("|", StringSplitOptions.RemoveEmptyEntries).Select(c => Util.CreateKeyValue(c.Split("=", StringSplitOptions.RemoveEmptyEntries))),
                        Indexed = bool.Parse(values[10]),
                        ManufacturingSortOrderId = values[11],
                        IsolateInSummary = bool.TryParse(values[12], out var isolateInSummary) ? isolateInSummary : false,
                        PreviewZoom = string.IsNullOrEmpty(values[13]) ? (Zoom?)null : (Zoom)Enum.Parse(typeof(Zoom), values[13])
                    };
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return componentImport;
            }
            
            public string TabTitle => "Component";
        }
    }
}