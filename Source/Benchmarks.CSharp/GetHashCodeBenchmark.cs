using System.Diagnostics.CodeAnalysis;
using BenchmarkDotNet.Attributes;

namespace Benchmarks.CSharp;

/// <summary>
/// GetHashCodeのベンチマーク
/// </summary>
[Config(typeof(BenchmarkConfig))]
public class GetHashCodeBenchmark
{
    public long Property1 { get; set; } = long.MaxValue;

    public string Property2 { get; set; } = "abcdefghij";

    public double Property3 { get; set; } = double.MaxValue;

    public int Property4 { get; set; } = 1;

    [Benchmark]
    [SuppressMessage("Style", "IDE0050:タプルに変換")]
    public int AnonymousType()
        => new { Property1, Property2, Property3, Property4 }.GetHashCode();

    [Benchmark]
    public int ValueTuple()
        => (Property1, Property2, Property3, Property4).GetHashCode();

    [Benchmark]
    public int ReSharper()
    {
        unchecked
        {
            var hash = Property1.GetHashCode();
            hash = (hash * 397) ^ (Property2?.GetHashCode(StringComparison.Ordinal) ?? 0);
            hash = (hash * 397) ^ Property3.GetHashCode();
            hash = (hash * 397) ^ Property4;
            return hash;
        }
    }

    [Benchmark]
    public int HashHelpers()
    {
        var hash = Property1.GetHashCode();
        hash = Combine(hash, Property2?.GetHashCode(StringComparison.Ordinal) ?? 0);
        hash = Combine(hash, Property3.GetHashCode());
        hash = Combine(hash, Property4);
        return hash;
    }

    [Benchmark(Baseline = true)]
    public int SystemHashCode()
        => HashCode.Combine(Property1, Property2, Property3, Property4);

    // cf. https://github.com/dotnet/corefx/blob/v2.2.0/src/Common/src/System/Numerics/Hashing/HashHelpers.cs
    static int Combine(int h1, int h2)
    {
        var rol5 = ((uint)h1 << 5) | ((uint)h1 >> 27);
        return ((int)rol5 + h1) ^ h2;
    }
}