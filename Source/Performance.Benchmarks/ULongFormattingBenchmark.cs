﻿using System.Buffers.Text;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using BenchmarkDotNet.Attributes;

namespace Performance.Benchmarks;

/// <summary>
/// ulongをUTF-8のbyte配列に変換する処理のベンチマーク
/// </summary>
public class ULongFormattingBenchmark
{
    const int BufferLength = 20;

    public static IEnumerable<ulong> Values()
    {
        yield return (ulong)DateTimeOffset.Parse("2018/01/01T00:00:00Z", CultureInfo.InvariantCulture).ToUnixTimeMilliseconds();
    }

    [Benchmark]
    [ArgumentsSource(nameof(Values))]
    public unsafe byte[] TryFormatEncodingGetBytes(ulong value)
    {
        Span<char> charBuffer = stackalloc char[BufferLength];
        value.TryFormat(charBuffer, out var charLength, null, CultureInfo.InvariantCulture);

        var byteBuffer = new byte[charLength];
        fixed (char* chars = charBuffer)
        {
            fixed (byte* bytes = byteBuffer)
            {
                Encoding.ASCII.GetBytes(chars, charLength, bytes, BufferLength);
            }
        }

        return byteBuffer;
    }

    [Benchmark]
    [ArgumentsSource(nameof(Values))]
    public byte[] Utf8FormatterTryFormat(ulong value)
    {
        Span<byte> byteBuffer = stackalloc byte[BufferLength];
        Utf8Formatter.TryFormat(value, byteBuffer, out var byteLength);
        return byteBuffer[..byteLength].ToArray();
    }

    [Benchmark]
    [ArgumentsSource(nameof(Values))]
    public byte[] ToUtf8Bytes(ulong value)
    {
        var digits = value < 10_000_000_000_000 ? 13
            : value < 100_000_000_000_000 ? 14
            : value < 1_000_000_000_000_000 ? 15
            : value < 10_000_000_000_000_000 ? 16
            : value < 100_000_000_000_000_000 ? 17
            : value < 1_000_000_000_000_000_000 ? 18
            : value < 10_000_000_000_000_000_000 ? 19
            : 20;

        var byteBuffer = new byte[digits];
        ref var byteBufferStart = ref MemoryMarshal.GetArrayDataReference(byteBuffer);

        for (var i = digits; i > 0; i--)
        {
            var temp = '0' + value;
            value /= 10;
            Unsafe.Add(ref byteBufferStart, i - 1) = (byte)(temp - (value * 10));
        }

        return byteBuffer;
    }
}
