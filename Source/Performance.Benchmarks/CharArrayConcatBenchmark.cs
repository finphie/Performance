﻿using System.Text;
using BenchmarkDotNet.Attributes;

namespace Performance.Benchmarks;

/// <summary>
/// char配列をUTF-16文字列に変換
/// </summary>
public class CharArrayConcatBenchmark
{
    char[] _source;

    [Params(10, 100, 512, 1024, 2048, 10000)]
    public int ArraySize { get; set; }

    [GlobalSetup]
    public void Setup() => _source = [.. Enumerable.Repeat('a', ArraySize)];

    [Benchmark]
    public string NewString() => new(_source);

    [Benchmark]
    public string StringConcat() => string.Concat(_source);

    [Benchmark]
    public string StringJoin() => string.Join(string.Empty, _source);

    [Benchmark]
    public string SpanToString() => _source.AsSpan().ToString();

    [Benchmark]
    public string StringBuilder()
    {
        var sb = new StringBuilder(_source.Length);
        foreach (var c in _source)
        {
            sb.Append(c);
        }

        return sb.ToString();
    }
}
