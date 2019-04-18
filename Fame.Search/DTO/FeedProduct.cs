using System;
using System.Collections.Generic;

namespace Fame.Search.DTO
{
    public class FeedProduct
    {
        public string SKU { get; set; }

        public string Title { get; set; }

        public string LinkAU { get; set; }

        public string LinkUS { get; set; }

        public string Description { get; set; }

        public string Keywords { get; set; }

        public Decimal PriceAU { get; set; }

        public Decimal PriceUS { get; set; }

        public string Image { get; set; }

        public string SizesAU { get; set; }

        public string SizesUS { get; set; }

        public string Silhouette { get; set; }

        public string Occasions { get; set; }

        public string Length { get; set; }

        public string Color { get; set; }
    }
}
