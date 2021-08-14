using System.Text;
using System.Text.Unicode;
using BenchmarkDotNet.Attributes;

namespace Benchmarks.CSharp;

/// <summary>
/// UTF-16文字列からUTF-8Byte配列に変換
/// </summary>
[Config(typeof(BenchmarkConfig))]
public class GetBytesBenchmark
{
    readonly string _data = Guid.NewGuid().ToString();
    readonly byte[] _buffer;

    public GetBytesBenchmark() => _buffer = new byte[_data.Length * 3];

    [Benchmark]
    public void Array() => Encoding.UTF8.GetBytes(_data, 0, _data.Length, _buffer, 0);

    [Benchmark]
    public void Span() => Encoding.UTF8.GetBytes(_data, _buffer);

    [Benchmark]
    public unsafe void UnsafeSpan()
    {
        fixed (char* chars = _data)
        {
            fixed (byte* bytes = _buffer.AsSpan())
            {
                Encoding.UTF8.GetBytes(chars, _data.Length, bytes, _buffer.Length);
            }
        }
    }

    [Benchmark]
    public unsafe void Unsafe()
    {
        fixed (char* chars = _data)
        {
            fixed (byte* bytes = _buffer)
            {
                Encoding.UTF8.GetBytes(chars, _data.Length, bytes, _buffer.Length);
            }
        }
    }

    [Benchmark]
    public void OperationStatusBased()
        => _ = Utf8.FromUtf16(_data, _buffer, out _, out _);
}