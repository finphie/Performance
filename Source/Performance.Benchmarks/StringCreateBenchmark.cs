using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using BenchmarkDotNet.Attributes;

namespace Benchmarks.CSharp;

/// <summary>
/// char配列から2文字を抽出してstringに変換
/// </summary>
[Config(typeof(BenchmarkConfig))]
public class StringCreateBenchmark
{
    char[] _source;

    [GlobalSetup]
    public void Setup() => _source = "0123456789abcdef".ToCharArray();

    [Benchmark]
    public string StringCreate()
        => string.Create(2, _source, (span, c) =>
        {
            span[0] = c[1];
            span[1] = c[10];
        });

    [Benchmark]
    public string New() => new(new[] { _source[1], _source[10] });

    [Benchmark]
    public unsafe string Stackalloc()
    {
        var array = stackalloc char[]
        {
            _source[1],
            _source[10]
        };
        return new(array, 0, 2);
    }

    [Benchmark]
    public string SpanStackalloc()
    {
        Span<char> span = stackalloc char[]
        {
            _source[1],
            _source[10]
        };
        return new(span);
    }

    [Benchmark]
    public unsafe string UnsafePointer()
    {
        var s = new string(default, 2);
        fixed (char* pointer = s)
        {
            pointer[0] = _source[1];
            pointer[1] = _source[10];
        }

        return s;
    }

    [Benchmark]
    public unsafe string UnsafeAsPointer()
    {
        var s = new string(default, 2);
        var handle = GCHandle.Alloc(s, GCHandleType.Pinned);
        var pointer = (char*)Unsafe.AsPointer(ref MemoryMarshal.GetReference(s.AsSpan()));
        pointer[0] = _source[1];
        pointer[1] = _source[10];
        handle.Free();
        return s;
    }

    [Benchmark]
    public unsafe string MemoryPin()
    {
        var s = new string(default, 2);
        var memory = s.AsMemory();
        using var handle = memory.Pin();

        var pointer = (char*)handle.Pointer;
        pointer[0] = _source[1];
        pointer[1] = _source[10];

        return s;
    }

    // cf. https://github.com/dotnet/corefx/blob/v2.2.0/src/Common/src/CoreLib/System/Char.cs#L994-L998
    [Benchmark]
    public unsafe string CoreFx()
    {
        var temp = 0U;
        var array = (char*)&temp;
        array[0] = _source[1];
        array[1] = _source[10];
        return new(array, 0, 2);
    }

    [Benchmark]
    public string MemoryMarshalCreateSpan()
    {
        var s = new string(default, 2);
        var span = MemoryMarshal.CreateSpan(ref MemoryMarshal.GetReference(s.AsSpan()), 2);
        span[0] = _source[1];
        span[1] = _source[10];
        return s;
    }

    [Benchmark]
    public string UnsafeAdd()
    {
        var s = new string(default, 2);
        ref var start = ref MemoryMarshal.GetReference(s.AsSpan());
        Unsafe.Add(ref start, 0) = _source[1];
        Unsafe.Add(ref start, 1) = _source[10];
        return s;
    }

    [Benchmark]
    public string StringBuilder()
        => new StringBuilder(2).Append(_source[1]).Append(_source[10]).ToString();
}
