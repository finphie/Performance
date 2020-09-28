using System.Runtime.Serialization;

namespace Benchmarks.CSharp
{
    enum TestEnum
    {
        [EnumMember(Value = "a")]
        A,

        [EnumMember(Value = "a")]
        B
    }
}