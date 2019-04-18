using System;
using System.Collections.Generic;

namespace Fame.Search.DTO
{
    [Serializable]
    public class ProductSummary
    {
        public string PID { get; set; }

        public string ComponentIdPath { get; set; }

        public string ProductId { get; set; }

        public string Name { get; set; }

        public Dictionary<string, int> Price { get; set; }
        
        public int ProductVersionId { get; set; }

        public decimal SortWeight { get; set; }

        //Diagnostic fields 
        //public int Index { get; set; }
        //public List<string> Facets { get; set; }
    }
}
