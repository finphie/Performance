using System;
using System.Collections.Concurrent;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace Benchmarks.CSharp.EnumGetEnumMember
{
    static class EnumHelperConcurrentDictionaryBaseEnumKey
    {
        static readonly ConcurrentDictionary<Enum, string> Dic = new();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string GetEnumMemberValue<T>(T value)
            where T : struct, Enum
            => Dic.GetOrAdd(value, e =>
                typeof(T)
                    .GetField(e.ToString())
                    .GetCustomAttribute<EnumMemberAttribute>().Value);
    }
}