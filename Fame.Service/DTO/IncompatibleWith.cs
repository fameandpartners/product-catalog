using System;
using System.Collections.Generic;

namespace Fame.Service.DTO
{
    [Serializable]
    public class IncompatibleWith
    {
        public string ComponentId { get; set; }
        public List<List<string>> Incompatibilities { get; set; }
    }
}