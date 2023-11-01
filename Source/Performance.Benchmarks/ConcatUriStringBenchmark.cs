using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Web;
using BenchmarkDotNet.Attributes;

namespace Performance.Benchmarks;

/// <summary>
/// URI文字列連結処理のベンチマーク
/// </summary>
public class ConcatUriStringBenchmark
{
    const char CharAndSign = '&';
    const char CharEqualsSign = '=';

    string _uri = null!;

    string _key1 = null!;
    string _key2 = null!;

    string _value1 = null!;
    string _value2 = null!;

    [GlobalSetup]
    public void Setup()
    {
        _uri = "uri?";

        _key1 = "key1";
        _key2 = "key2";

        _value1 = "value1";
        _value2 = "value2";
    }

    [Benchmark]
    public string HttpUtilityParseQueryString()
    {
        var query = HttpUtility.ParseQueryString(string.Empty);
        query[_key1] = _value1;
        query[_key2] = _value2;

        return _uri + query;
    }

    [Benchmark]
    public string UnsafeCopyBlockUnaligned()
    {
        var length = _uri.Length +
                     _key1.Length + _value1.Length +
                     _key2.Length + _value2.Length +
                     3;

        var result = new string(default, length);
        ref var resultStart = ref Unsafe.As<char, byte>(ref MemoryMarshal.GetReference(result.AsSpan()));

        var pos = _uri.Length * sizeof(char);

        ref var sourceStart = ref Unsafe.As<char, byte>(ref MemoryMarshal.GetReference(_uri.AsSpan()));
        Unsafe.CopyBlockUnaligned(ref resultStart, ref sourceStart, (uint)pos);

        var size = _key1.Length * sizeof(char);
        sourceStart = ref Unsafe.As<char, byte>(ref MemoryMarshal.GetReference(_key1.AsSpan()));
        Unsafe.CopyBlockUnaligned(ref Unsafe.Add(ref resultStart, pos), ref sourceStart, (uint)size);
        pos += size;

        Unsafe.Add(ref resultStart, pos) = (byte)CharEqualsSign;
        pos += sizeof(char);

        size = _value1.Length * sizeof(char);
        sourceStart = ref Unsafe.As<char, byte>(ref MemoryMarshal.GetReference(_value1.AsSpan()));
        Unsafe.CopyBlockUnaligned(ref Unsafe.Add(ref resultStart, pos), ref sourceStart, (uint)size);
        pos += size;

        Unsafe.Add(ref resultStart, pos) = (byte)CharAndSign;
        pos += sizeof(char);

        size = _key2.Length * sizeof(char);
        sourceStart = ref Unsafe.As<char, byte>(ref MemoryMarshal.GetReference(_key2.AsSpan()));
        Unsafe.CopyBlockUnaligned(ref Unsafe.Add(ref resultStart, pos), ref sourceStart, (uint)size);
        pos += size;

        Unsafe.Add(ref resultStart, pos) = (byte)CharEqualsSign;
        pos += sizeof(char);

        size = _value2.Length * sizeof(char);
        sourceStart = ref Unsafe.As<char, byte>(ref MemoryMarshal.GetReference(_value2.AsSpan()));
        Unsafe.CopyBlockUnaligned(ref Unsafe.Add(ref resultStart, pos), ref sourceStart, (uint)size);

        return result;
    }
}
