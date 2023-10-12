using System.Collections;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace Performance.Benchmarks.EnumGetEnumMember;

static class EnumHelperHashtable
{
#pragma warning disable SA1010 // Opening square brackets should be spaced correctly
    static readonly Hashtable Dic = [];
#pragma warning restore SA1010 // Opening square brackets should be spaced correctly

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string GetEnumMemberValue<T>(T value)
        where T : struct, Enum
    {
        if (Dic.ContainsKey(value))
        {
            return (Dic[value] as string)!;
        }

        var memberValue = typeof(T)
            .GetField(value.ToString())
            !.GetCustomAttribute<EnumMemberAttribute>()!.Value;
        Dic[value] = memberValue;
        return memberValue!;
    }
}
