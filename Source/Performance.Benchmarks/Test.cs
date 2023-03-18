using System.Runtime.Serialization;

namespace Performance.Benchmarks;

enum Test
{
    [EnumMember(Value = "a")]
    A,

    [EnumMember(Value = "b")]
    B
}
