using Fame.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fame.Search.Models
{
    [Serializable]
    public class SearchMeta : ISearchMeta
    {
        public string ProductDocumentVersionId { get; set; }
        public string PrimarySilhouetteId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string TaxonString { get; set; }
    }
}
