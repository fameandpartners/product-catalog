using System;
using System.Runtime.Serialization;

namespace Fame.Data.Models
{
    [Serializable]
    public enum Orientation
    {
        [EnumMember(Value = "front")] Front,
        [EnumMember(Value = "back")] Back,
        [EnumMember(Value = "swatch")] Swatch,
        [EnumMember(Value = "cad")] Cad
    }
}