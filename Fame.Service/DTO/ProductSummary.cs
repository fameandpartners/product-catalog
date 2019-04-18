using System;
using System.Collections.Generic;
using Fame.Data.Models;

namespace Fame.Service.DTO
{
    [Serializable]
	public class CurationMeta
	{
		public string Name { get; set; }
		public string Description { get; set; }
		public string StyleDescription { get; set; }
		public string Keywords { get; set; }
		public string PermaLink { get; set; }
	}
    
    [Serializable]
	public class LayerCad
	{
		public string Url { get; set; }
		public double Width { get; set; }
		public double Height { get; set; }
		public int SortOrder { get; set; }
		public string Type { get; set; }
		public List<string> Components { get; set; }
	}
    
    [Serializable]
	public class ProductSize
	{
		public int MinHeightCm { get; set; }
		public int MaxHeightCm { get; set; }
		public int MinHeightInch { get; set; }
		public int MaxHeightInch { get; set; }
		public string SizeChart { get; set; }
	}

    [Serializable]
	public class ProductSummary
	{
		public string ProductId { get; set; }

	    public ProductType ProductType { get; set; }

        public int ProductVersionId { get; set; }

		public string CartId { get; set; }

		public PreviewType PreviewType { get; set; }

		public bool IsAvailable { get; set; }
        
		public string ReturnDescription => @"Shipping is free on your customized item. <a href='/faqs#panel-delivery' target='_blank'>Learn more</a>";

		public string DeliveryTimeDescription => "Estimated Delivery 6-10 weeks.";

		public List<string> Curations { get; set; }

		public decimal Price { get; set; }

		public Dictionary<string, bool> PaymentMethods { get; set; }

		public ProductSize Size { get; set; }

		public List<MediaListItem> Media { get; set; }

		public List<string> ProductRenderComponents { get; set; }

		public List<RenderPositionSummary> RenderPositions { get; set; }

		public List<ComponentSummary> Components { get; set; }

		public List<GroupSummary> Groups { get; set; }

		public CurationMeta CurationMeta { get; set; }

		public List<LayerCad> LayerCads { get; set; }
	}
}