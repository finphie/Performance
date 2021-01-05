using System.Runtime.Serialization;

namespace Benchmarks.CSharp
{
    enum Test
    {
        [EnumMember(Value = "a")]
        A,

        [EnumMember(Value = "b")]
        B
    }
}