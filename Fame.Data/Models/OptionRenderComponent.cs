namespace Fame.Data.Models
{
    [System.Serializable]
    public class OptionRenderComponent
    {
        public string ComponentTypeId { get; set; }
        public int OptionId { get; set; }
        
        public ComponentType ComponentType { get; set; }
        public Option Option { get; set; }
    }
}