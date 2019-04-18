using System.Runtime.Serialization;

namespace Fame.Search.DTO
{
    public enum SortOrder
    {
        [EnumMember(Value = "asc")] Ascending,
        [EnumMember(Value = "desc")] Descending,
    }
}