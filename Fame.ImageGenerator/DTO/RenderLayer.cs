using System;
using System.Collections.Generic;
using Fame.Service.DTO;

namespace Fame.ImageGenerator.DTO
{
    [Serializable]
    public class RenderLayer
    {
        public FileMeta File { get; set; }
        public int ZIndex { get; set; }
        public ISet<ISet<string>> Combinations { get; set; }
        public bool AlwaysInclude { get; set; }
    }
}
