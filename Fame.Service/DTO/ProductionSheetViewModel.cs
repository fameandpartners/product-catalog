using System;
using System.Collections.Generic;
using System.Linq;
using Fame.Data.Models;

namespace Fame.Service.DTO
{
    [Serializable]
    public class ProductionSheetViewModel
    {
        private readonly string _defaultColorComponentId;
        private readonly string _renderUrlPrefix;
        private readonly int _productVersionId;

        public ProductionSheetViewModel(
            string productId, 
            string productTitle, 
            string defaultColorComponentId, 
            List<ComponentViewModel> components, 
            string renderUrlPrefix, 
            int productVersionId)
        {
            ProductId = productId;
            ProductTitle = productTitle;
            _defaultColorComponentId = defaultColorComponentId;
            Components = components;
            _renderUrlPrefix = renderUrlPrefix;
            _productVersionId = productVersionId;
        }

        public string ProductId { get; private set; }

        public string ProductTitle { get; private set; }

        public List<ComponentViewModel> Components { get; set; }

        public string Front => GenerateImageUrl(Orientation.Front, Zoom.None, Size.SIZE_OPTION_2816.ToString(), Components.Select(c => c.ComponentId));

        public string Back => GenerateImageUrl(Orientation.Back, Zoom.None, Size.SIZE_OPTION_2816.ToString(), Components.Select(c => c.ComponentId));

        public string FrontDefaultColor => GenerateImageUrl(Orientation.Front, Zoom.None, Size.SIZE_OPTION_2816.ToString(), GetDefaultColorComponentIds());

        public string BackDefaultColor => GenerateImageUrl(Orientation.Back, Zoom.None, Size.SIZE_OPTION_2816.ToString(), GetDefaultColorComponentIds());

        public List<string> Extras
        {
            get
            {
                var lst = new List<string>();
                foreach(var extra in Components.Where(c => c.ComponentTypeId == "extra"))
                { 
                    var componentIds = Components.Where(c => c.ComponentTypeId == "fabric").Select(c => c.ComponentId).ToList();
                    componentIds.Add(extra.ComponentId);
                    componentIds.Add(_defaultColorComponentId);
                    lst.Add(GenerateImageUrl(Orientation.Front, extra.PreviewZoom, Size.SIZE_OPTION_704.ToString(), componentIds.OrderBy(c => c)));
                    lst.Add(GenerateImageUrl(Orientation.Back, extra.PreviewZoom, Size.SIZE_OPTION_704.ToString(), componentIds.OrderBy(c => c)));
                }
                return lst;
            }
        }

        private List<string> GetDefaultColorComponentIds()
        {
            var currentColor = Components.SingleOrDefault(c => c.ComponentTypeId == "color");
            var componentIds = Components.Where(c => !c.IsolateInSummary).Select(c => c.ComponentId).ToList();
            if (currentColor == null || currentColor.ComponentId == _defaultColorComponentId) return componentIds;
            return componentIds.Select(s => s == currentColor.ComponentId ? _defaultColorComponentId : s).ToList();
        }

        private string GenerateImageUrl(Orientation orientation, Zoom? zoom, string imageSize, IEnumerable<string> componentIds)
        {
            return $@"{_renderUrlPrefix}/{ProductId}/{orientation}{(zoom ?? Zoom.None)}/{imageSize}/{string.Join('~', componentIds)}.png?pvid={_productVersionId}"; 
        }
    }
}