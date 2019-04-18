namespace Fame.Data.Models
{
    [System.Serializable]
    public class ProductRenderComponent
    {
        public string ComponentTypeId { get; set; }
        public int ProductVersionId { get; set; }
        
        public ComponentType ComponentType { get; set; }
        public ProductVersion ProductVersion { get; set; }
    }
}