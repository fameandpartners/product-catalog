using System;

namespace Fame.Data.Models
{
    [Serializable]
    public class ComponentMeta
    {
        public string ComponentId { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }

        public Component Component { get; set; }
    }
}