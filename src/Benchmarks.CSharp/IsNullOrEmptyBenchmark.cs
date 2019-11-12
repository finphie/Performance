using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using BenchmarkDotNet.Attributes;

namespace Benchmarks.CSharp
{
    /// <summary>
    /// IsNullOrEmptyのベンチマーク
    /// </summary>
    [Config(typeof(BenchmarkConfig))]
    [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "ベンチマーク")]
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
            => value is null || (uint)value.Length <= 0U;

        [Benchmark]
        [ArgumentsSource(nameof(Values))]
        public bool IsNullOrEmpty2(string value)
            => (value is null || (uint)value.Length <= 0U) ? true : false;

        [Benchmark]
        [ArgumentsSource(nameof(Values))]
        public bool IsNullOrEmpty3(string value)
            => (value?.Length ?? 0) == 0;

        [Benchmark]
        [ArgumentsSource(nameof(Values))]
        public bool IsNullOrEmpty4(string value)
            => (value?.Length ?? 0) == 0 ? true : false;
    }
}