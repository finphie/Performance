using System.IO.Hashing;
using BenchmarkDotNet.Attributes;
using CommunityToolkit.HighPerformance;
using CommunityToolkit.HighPerformance.Helpers;

namespace Performance.Benchmarks;

[Config(typeof(BenchmarkConfig))]
public class ArrayHashCodeBenchmark
{
    byte[] _data = null!;

    [Params(10, 100, 1024)]
    public int ArraySize { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        _data = new byte[ArraySize];
        Random.Shared.NextBytes(_data);
    }

    [Benchmark]
    public int HashCode()
    {
        var hash = default(HashCode);
        hash.AddBytes(_data);
        return hash.ToHashCode();
    }

    [Benchmark]
    public int XxHash()
    {
        var hash = XxHash3.HashToUInt64(_data);
        return (int)hash ^ (int)(hash >> 32);
    }

    [Benchmark]
    public int CommunityToolkitHashCode()
    {
        var hash = default(HashCode);
        hash.Add<byte>(_data);
        return hash.ToHashCode();
    }

    [Benchmark]
    public int CommunityToolkitHashCodeCombine() => HashCode<byte>.Combine(_data);

    [Benchmark]
    public int CommunityToolkitDjb2HashCode() => _data.GetDjb2HashCode();
}
