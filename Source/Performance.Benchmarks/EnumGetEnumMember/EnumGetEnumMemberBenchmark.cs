using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using BenchmarkDotNet.Attributes;
using EnumsNET;
using FastEnumUtility;

namespace Performance.Benchmarks.EnumGetEnumMember;

/// <summary>
/// EnumMemberを取得する処理のベンチマーク
/// </summary>
public class EnumGetEnumMemberBenchmark
{
    public EnumGetEnumMemberBenchmark()
        => RuntimeHelpers.RunClassConstructor(typeof(EnumMemberCache<Test>).TypeHandle);

    [Benchmark]
    public string Standard()
        => EnumHelper.GetEnumMemberValue(Test.A);

    [Benchmark]
    public string ConcurrentDictionaryBaseEnumKey()
        => EnumHelperConcurrentDictionaryBaseEnumKey.GetEnumMemberValue(Test.A);

    [Benchmark]
    public string ConcurrentDictionaryEnumKey()
        => EnumHelperConcurrentDictionaryEnumKey.GetEnumMemberValue(Test.A);

    [Benchmark]
    public string Hashtable()
        => EnumHelperHashtable.GetEnumMemberValue(Test.A);

    [Benchmark]
    public string Array()
        => EnumMemberCache<Test>.Get(Test.A);

    [Benchmark]
    public string EnumsNet()
        => Test.A.GetAttributes()!.Get<EnumMemberAttribute>()!.Value!;

    [Benchmark]
    public string FastEnum()
        => Test.A.GetEnumMemberValue()!;
}
