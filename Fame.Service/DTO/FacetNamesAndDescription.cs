namespace Fame.Service.DTO
{
    public class FacetPriority
    {
        public string FacetId { get; set; }
        public int TagPriority { get; set; }
        public int FacetGroupOrder { get; set; }
        public bool IsPrimarySilhouette { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
