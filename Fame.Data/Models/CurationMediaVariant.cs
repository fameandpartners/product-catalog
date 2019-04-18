using System;

namespace Fame.Data.Models
{
    [Serializable]
	public class CurationMediaVariant
    {
        public int Id { get; set; }
        public int CurationMediaId { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string Ext { get; set; }
        public bool IsOriginal { get; set; }
        public int Quality { get; set; }

        public CurationMedia CurationMedia { get; set; }
    }
}