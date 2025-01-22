using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using BenchmarkDotNet.Attributes;

namespace Performance.Benchmarks;

/// <summary>
/// ASCII文字列をUTF-8のbyte配列に変換する処理のベンチマーク
/// </summary>
public class AsciiStringFormattingBenchmark
{
    public static IEnumerable<string> Values()
    {
        yield return DateTimeOffset.Parse("2018/01/01T00:00:00Z", CultureInfo.InvariantCulture).ToUnixTimeMilliseconds().ToString(CultureInfo.InvariantCulture);
    }

    [Benchmark]
    [ArgumentsSource(nameof(Values))]
    public unsafe byte[] EncodingGetBytes(string value)
    {
        var byteBuffer = new byte[value.Length];
        fixed (char* chars = value)
        {
            fixed (byte* bytes = byteBuffer)
            {
                Encoding.ASCII.GetBytes(chars, value.Length, bytes, byteBuffer.Length);
            }
        }

        return byteBuffer;
    }

    [Benchmark]
    [ArgumentsSource(nameof(Values))]
    public byte[] Cast1(string value)
    {
        ref var valueStart = ref MemoryMarshal.GetReference(value.AsSpan());
        var byteBuffer = new byte[value.Length];
        for (var i = 0; i < byteBuffer.Length; i++)
        {
            byteBuffer[i] = (byte)Unsafe.Add(ref valueStart, i);
        }

        return byteBuffer;
    }

    [Benchmark]
    [ArgumentsSource(nameof(Values))]
    public byte[] Cast2(string value)
    {
        ref var valueStart = ref MemoryMarshal.GetReference(value.AsSpan());
        var byteBuffer = new byte[value.Length];
        for (var i = 0; i < byteBuffer.Length; i++)
        {
            byteBuffer[i] = (byte)Unsafe.AddByteOffset(ref valueStart, i * sizeof(char));
        }

        return byteBuffer;
    }

    [Benchmark]
    [ArgumentsSource(nameof(Values))]
    public byte[] Cast3(string value)
    {
        ref var valueStart = ref MemoryMarshal.GetReference(value.AsSpan());
        var byteBuffer = new byte[value.Length];

        for (var i = 0; i < byteBuffer.Length; i++)
        {
            byteBuffer[i] = (byte)valueStart;
            valueStart = ref Unsafe.AddByteOffset(ref valueStart, sizeof(char));
        }

        return byteBuffer;
    }
}
