using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using BenchmarkDotNet.Attributes;

namespace Performance.Benchmarks;

/// <summary>
/// byte配列をコピーする処理のベンチマーク
/// </summary>
public class ArrayCopyBenchmark
{
    byte[] _source;
    byte[] _destination;

    [Params(10, 100, 1000, 10000, 100000, 1000000)]
    public int ArraySize { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        _source = Enumerable.Repeat<byte>(1, ArraySize).ToArray();
        _destination = new byte[_source.Length];
    }

    [Benchmark]
    public void ArrayCopy()
        => Array.Copy(_source, 0, _destination, 0, _destination.Length);

    [Benchmark]
    public void SpanCopyTo()
        => _source.AsSpan().CopyTo(_destination);

    [Benchmark]
    public void MemoryCopyTo()
        => _source.AsMemory().CopyTo(_destination);

    [Benchmark]
    public void BufferBlockCopy()
        => Buffer.BlockCopy(_source, 0, _destination, 0, _destination.Length);

    [Benchmark]
    public unsafe void BufferMemoryCopy()
    {
        fixed (void* source = _source, destination = _destination)
        {
            Buffer.MemoryCopy(source, destination, _destination.Length, _source.Length);
        }
    }

    [Benchmark]
    public unsafe void MarshalCopy()
    {
        fixed (byte* source = _source)
        {
            Marshal.Copy((IntPtr)source, _destination, 0, _destination.Length);
        }
    }

    [Benchmark]
    public void UnsafeCopyBlock()
        => Unsafe.CopyBlock(ref MemoryMarshal.GetArrayDataReference(_destination), ref MemoryMarshal.GetArrayDataReference(_source), (uint)_destination.Length);

    [Benchmark]
    public void UnsafeCopyBlockUnaligned()
        => Unsafe.CopyBlockUnaligned(ref MemoryMarshal.GetArrayDataReference(_destination), ref MemoryMarshal.GetArrayDataReference(_source), (uint)_destination.Length);

    [Benchmark]
    public unsafe void UnmanagedMemoryStreamCopyTo()
    {
        var length = _destination.Length;
        fixed (byte* source = _source, destination = _destination)
        {
            using var streamSource = new UnmanagedMemoryStream(source, _source.Length);
            using var streamDestination = new UnmanagedMemoryStream(destination, length, length, FileAccess.Write);
            streamSource.CopyTo(streamDestination);
        }
    }

    [Benchmark]
    public unsafe void UnmanagedMemoryStreamRead()
    {
        fixed (byte* source = _source)
        {
            using var streamSource = new UnmanagedMemoryStream(source, _source.Length);
            streamSource.Read(_destination);
        }
    }

    [Benchmark]
    public unsafe void UnmanagedMemoryStreamWrite()
    {
        var length = _destination.Length;
        fixed (byte* destination = _destination)
        {
            using var streamDestination = new UnmanagedMemoryStream(destination, length, length, FileAccess.Write);
            streamDestination.Write(_source);
        }
    }
}
