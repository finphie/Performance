using BenchmarkDotNet.Attributes;

namespace Performance.Benchmarks;

/// <summary>
/// IsNullOrEmptyのベンチマーク
/// </summary>
public class IsNullOrEmptyBenchmark
{
    public static IEnumerable<string?> Values()
    {
        yield return null;
        yield return string.Empty;
        yield return "abc";
    }

    [Benchmark]
    [ArgumentsSource(nameof(Values))]
    public bool IsNullOrEmpty1(string value)
      => string.IsNullOrEmpty(value);

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
    public bool IsNullOrEmptySpan(ReadOnlySpan<char> value)
        => value.IsEmpty;
}
