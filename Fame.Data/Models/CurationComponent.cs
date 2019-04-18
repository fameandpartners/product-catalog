using System;

namespace Fame.Data.Models
{
    [Serializable]
    public class CurationComponent
    {
        public string PID { get; set; }
        public string ComponentId { get; set; }

        public Curation Curation { get; set; }
        public Component Component { get; set; }
    }
}
