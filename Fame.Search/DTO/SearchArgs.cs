using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fame.Search.DTO
{
    public class SearchArgs
    {
        private List<string> _facets;

        public SearchArgs()
        {
            Facets = new List<string>();
        }

        public int PageSize { get; set; }

        public string SortField { get; set; }

        public SortOrder? SortOrder { get; set; }

        public List<string> Facets
        {
            get => _facets.Select(f => f.ToLower()).ToList();
            set => _facets = value;
        }

        public long? LastIndex { get; set; }

        public decimal? LastValue { get; set; }

        public IList<object> SearchAfter => LastIndex.HasValue && LastValue.HasValue ? new List<object> { LastValue, LastIndex } : null;

        public List<string> Collections { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendJoin("-", Facets).AppendJoin("-", PageSize, SortField).AppendJoin("-", Collections);
            if (SortOrder.HasValue) sb.AppendJoin("-", SortOrder);
            if (LastValue.HasValue) sb.AppendJoin("-", LastValue);
            if (LastIndex.HasValue) sb.AppendJoin("-", LastIndex);
            return sb.ToString();
        }
    }
}
