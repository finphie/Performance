using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using BenchmarkDotNet.Attributes;

namespace Benchmarks.CSharp;

/// <summary>
/// stringやchar配列から、特定の要素2つにアクセスする処理のベンチマークです。
/// </summary>
[Config(typeof(BenchmarkConfig))]
public class CharArrayAccessBenchmark
{
    const int Count = 32;
    const string SourceConstString = "0123456789abcdef";

    static readonly char[] SourceChars =
    {
        '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
        'a', 'b', 'c', 'd', 'e', 'f'
    };

    [Params(3)]
    public int Index1 { get; set; }

    [Params(7)]
    public int Index2 { get; set; }

    [Benchmark]
    public int ConstString()
    {
        var result = 0;
        for (var i = 0; i < Count; i++)
        {
            result += SourceConstString[Index1] + SourceConstString[Index2];
        }

        return result;
    }

    [Benchmark]
    public int CharArray()
    {
        var result = 0;
        for (var i = 0; i < Count; i++)
        {
            result += SourceChars[Index1] + SourceChars[Index2];
        }

        return result;
    }

    [Benchmark]
    public unsafe int PointerConstString()
    {
        var result = 0;
        fixed (char* pointer = SourceConstString)
        {
            for (var i = 0; i < Count; i++)
            {
                result += pointer[Index1] + pointer[Index2];
            }
        }

        return result;
    }

    [Benchmark]
    public unsafe int PointerCharArray()
    {
        var result = 0;
        fixed (char* pointer = SourceChars)
        {
            for (var i = 0; i < Count; i++)
            {
                result += pointer[Index1] + pointer[Index2];
            }
        }

        return result;
    }

    [Benchmark]
    public unsafe int UnsafeAsPointerCharArray()
    {
        var handle = GCHandle.Alloc(SourceChars, GCHandleType.Pinned);
        var pointer = (char*)Unsafe.AsPointer(ref MemoryMarshal.GetReference(SourceChars.AsSpan()));
        var result = 0;
        for (var i = 0; i < Count; i++)
        {
            result += pointer[Index1] + pointer[Index2];
        }

        handle.Free();
        return result;
    }

    [Benchmark]
    public unsafe int MemoryPinConstString()
    {
        var memory = SourceConstString.AsMemory();
        var result = 0;
        using var handle = memory.Pin();
        var pointer = (char*)handle.Pointer;
        for (var i = 0; i < Count; i++)
        {
            result += pointer[Index1] + pointer[Index2];
        }

        return result;
    }

    [Benchmark]
    public unsafe int MemoryPinCharArray()
    {
        var memory = SourceChars.AsMemory();
        using var handle = memory.Pin();
        var pointer = (char*)handle.Pointer;
        var result = 0;
        for (var i = 0; i < Count; i++)
        {
            result += pointer[Index1] + pointer[Index2];
        }

        return result;
    }

    [Benchmark]
    public int SpanConstString()
    {
        var span = SourceConstString.AsSpan();
        var result = 0;
        for (var i = 0; i < Count; i++)
        {
            result += span[Index1] + span[Index2];
        }

        return result;
    }

    [Benchmark]
    public int SpanCharArray()
    {
        var span = SourceChars.AsSpan();
        var result = 0;
        for (var i = 0; i < Count; i++)
        {
            result += span[Index1] + span[Index2];
        }

        return result;
    }

    [Benchmark]
    public int UnsafeAddConstString()
    {
        ref var start = ref MemoryMarshal.GetReference(SourceConstString.AsSpan());
        var result = 0;
        for (var i = 0; i < Count; i++)
        {
            result += Unsafe.Add(ref start, Index1) + Unsafe.Add(ref start, Index2);
        }

        return result;
    }

    [Benchmark]
    public int UnsafeAddCharArray()
    {
        ref var start = ref SourceChars.AsSpan().GetPinnableReference();
        var result = 0;
        for (var i = 0; i < Count; i++)
        {
            result += Unsafe.Add(ref start, Index1) + Unsafe.Add(ref start, Index2);
        }

        return result;
    }
}
