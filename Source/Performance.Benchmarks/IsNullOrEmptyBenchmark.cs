using BenchmarkDotNet.Attributes;

namespace Performance.Benchmarks;

/// <summary>
/// IsNullOrEmptyのベンチマーク
/// </summary>
public class IsNullOrEmptyBenchmark
{
    public static IEnumerable<string?> Values => new[]
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
    public bool IsNullOrEmpty3(string value)
        => (value?.Length ?? 0) == 0;

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
