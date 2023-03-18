using System.Buffers.Text;
using System.Globalization;
using System.Text;
using BenchmarkDotNet.Attributes;

namespace Performance.Benchmarks;

/// <summary>
/// doubleをstringに変換
/// </summary>
[Config(typeof(BenchmarkConfig))]
public class DoubleFormattingBenchmark
{
    const int BufferSize = 32;

    [Params(12345.6789)]
    public double Value { get; set; }

    [Benchmark]
    public string DoubleToString()
        => Value.ToString(CultureInfo.InvariantCulture);

    [Benchmark]
    public byte[] DoubleToStringEncodingGetBytes()
        => Encoding.UTF8.GetBytes(Value.ToString(CultureInfo.InvariantCulture));

    [Benchmark]
    public byte[] DoubleToStringEncodingGetBytesSpan()
    {
        Span<byte> byteBuffer = stackalloc byte[BufferSize];
        var byteLength = Encoding.UTF8.GetBytes(Value.ToString(CultureInfo.InvariantCulture), byteBuffer);
        return byteBuffer[..byteLength].ToArray();
    }

    [Benchmark]
    public unsafe byte[] DoubleToStringEncodingGetBytesUnsafe()
    {
        var value = Value.ToString(CultureInfo.InvariantCulture);
        Span<byte> byteBuffer = stackalloc byte[BufferSize];
        fixed (char* chars = value)
        {
            fixed (byte* bytes = byteBuffer)
            {
                var byteLength = Encoding.UTF8.GetBytes(chars, value.Length, bytes, BufferSize);
                return byteBuffer[..byteLength].ToArray();
            }
        }
    }

    [Benchmark]
    public string DoubleTryFormat()
    {
        Span<char> charBuffer = stackalloc char[BufferSize];
        Value.TryFormat(charBuffer, out var charLength, null, CultureInfo.InvariantCulture);
        return new string(charBuffer[..charLength]);
    }

    [Benchmark]
    public byte[] DoubleTryFormatEncodingGetBytes()
    {
        Span<char> charBuffer = stackalloc char[BufferSize];
        Value.TryFormat(charBuffer, out var charLength, null, CultureInfo.InvariantCulture);
        return Encoding.UTF8.GetBytes(charBuffer[..charLength].ToArray());
    }

    [Benchmark]
    public byte[] DoubleTryFormatEncodingGetBytesSpan()
    {
        Span<char> charBuffer = stackalloc char[BufferSize];
        Value.TryFormat(charBuffer, out var charLength, null, CultureInfo.InvariantCulture);
        Span<byte> byteBuffer = stackalloc byte[charLength];
        var byteLength = Encoding.UTF8.GetBytes(charBuffer[..charLength], byteBuffer);
        return byteBuffer[..byteLength].ToArray();
    }

    [Benchmark]
    public unsafe byte[] DoubleTryFormatEncodingGetBytesUnsafe()
    {
        Span<char> charBuffer = stackalloc char[BufferSize];
        Value.TryFormat(charBuffer, out var charLength, null, CultureInfo.InvariantCulture);
        Span<byte> byteBuffer = stackalloc byte[charLength];
        fixed (char* chars = charBuffer)
        {
            fixed (byte* bytes = byteBuffer)
            {
                var byteLength = Encoding.UTF8.GetBytes(chars, charLength, bytes, BufferSize);
                return byteBuffer[..byteLength].ToArray();
            }
        }
    }

    [Benchmark]
    public string Utf8FormatterTryFormatEncodingGetString()
    {
        Span<byte> byteBuffer = stackalloc byte[BufferSize];
        Utf8Formatter.TryFormat(Value, byteBuffer, out var byteLength);
        return Encoding.UTF8.GetString(byteBuffer[..byteLength]);
    }

    [Benchmark]
    public byte[] Utf8FormatterTryFormat()
    {
        Span<byte> byteBuffer = stackalloc byte[BufferSize];
        Utf8Formatter.TryFormat(Value, byteBuffer, out var byteLength);
        return byteBuffer[..byteLength].ToArray();
    }
}
