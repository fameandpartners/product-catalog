using System;
using System.Collections.Generic;

namespace Fame.Data.Models
{
    [Serializable]
    public class ManufacturingSortOrder
    {
        public string Id { get; set; }

        public int Order { get; set; }

        public ICollection<Component> Components { get; set; }
    }
}