using System.Diagnostics.CodeAnalysis;
using BenchmarkDotNet.Attributes;

namespace Benchmarks.CSharp;

/// <summary>
/// IsNullOrEmptyのベンチマーク
/// </summary>
[Config(typeof(BenchmarkConfig))]
public class IsNullOrEmptyBenchmark
{
    public static IEnumerable<string> Values => new[]
    {
        null,
        string.Empty,
        "abc"
    };

    [Benchmark]
    [ArgumentsSource(nameof(Values))]
    public bool IsNullOrEmpty1(string value)
      => value is null || value.Length == 0;

    [Benchmark]
    [ArgumentsSource(nameof(Values))]
    public bool IsNullOrEmpty2(string value)
        => value is null || (uint)value.Length <= 0U;

    [Benchmark]
    [ArgumentsSource(nameof(Values))]
    [SuppressMessage("Style", "IDE0075:Simplify conditional expression")]
    public bool IsNullOrEmpty3(string value)
        => (value is null || (uint)value.Length <= 0U) ? true : false;

    [Benchmark]
    [ArgumentsSource(nameof(Values))]
    public bool IsNullOrEmpty4(string value)
        => (value?.Length ?? 0) == 0;

    [Benchmark]
    [ArgumentsSource(nameof(Values))]
    [SuppressMessage("Style", "IDE0075:Simplify conditional expression")]
    public bool IsNullOrEmpty5(string value)
        => (value?.Length ?? 0) == 0 ? true : false;

    [Benchmark]
    [Arguments("")]
    [Arguments("abc")]
    public bool IsNullOrEmptySpan1(ReadOnlySpan<char> value)
        => value == null || value.Length == 0;

    [Benchmark]
    [Arguments("")]
    [Arguments("abc")]
    public bool IsNullOrEmptySpan2(ReadOnlySpan<char> value)
        => value == null || (uint)value.Length <= 0U;

    [Benchmark]
    [Arguments("")]
    [Arguments("abc")]
    public bool IsNullOrEmptySpan3(ReadOnlySpan<char> value)
       => value == null || value.IsEmpty;
}