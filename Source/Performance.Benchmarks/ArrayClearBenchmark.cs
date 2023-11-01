using BenchmarkDotNet.Attributes;

namespace Performance.Benchmarks;

public class ArrayClearBenchmark
{
    byte[] _buffer = null!;

    [Params(10, 100, 1000)]
    public int Length { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        _buffer = new byte[Length];
        var random = new Random();
        random.NextBytes(_buffer);
    }

    [Benchmark]
    public byte[] ArrayClear()
    {
        Array.Clear(_buffer, 0, Length);
        return _buffer;
    }

    [Benchmark]
    public byte[] SpanClear()
    {
        _buffer.AsSpan().Clear();
        return _buffer;
    }
}
