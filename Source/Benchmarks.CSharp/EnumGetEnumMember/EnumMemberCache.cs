using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Benchmarks.CSharp.EnumGetEnumMember;

static class EnumMemberCache<T>
        where T : struct, Enum
{
    static readonly string[] Table;

    static EnumMemberCache()
    {
        var values = typeof(T).GetFields(BindingFlags.Public | BindingFlags.Static);
        Table = new string[values.Length];
        for (var i = 0; i < values.Length; i++)
        {
            Table[i] = values[i].GetCustomAttribute<EnumMemberAttribute>().Value;
        }
    }

    public static string Get(T value)
        => Unsafe.Add(ref MemoryMarshal.GetArrayDataReference(Table), Unsafe.As<T, int>(ref value));
}
