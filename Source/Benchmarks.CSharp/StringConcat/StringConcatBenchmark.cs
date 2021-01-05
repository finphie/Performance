using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using BenchmarkDotNet.Attributes;
using Benchmarks.CSharp.Extensions;
using Benchmarks.CSharp.Helpers;

namespace Benchmarks.CSharp.StringConcat
{
    /// <summary>
    /// 文字列連結処理のベンチマーク
    /// </summary>
    [Config(typeof(BenchmarkConfig))]
    public class StringConcatBenchmark
    {
        const string Count04 = "04";
        const string Count08 = "08";
        const string Count12 = "12";

        string _source00, _source01, _source02, _source03;
        string _source04, _source05, _source06, _source07;
        string _source08, _source09, _source10, _source11;

        [Params(5, 10, 16, 32)]
        public int Length { get; set; }

        [GlobalSetup]
        public void Setup()
        {
            string[] CreateUtf16Strings() => StringHelper.CreateUtf16Strings(4, Length);

            (_source00, _source01, _source02, _source03) = CreateUtf16Strings();
            (_source04, _source05, _source06, _source07) = CreateUtf16Strings();
            (_source08, _source09, _source10, _source11) = CreateUtf16Strings();
        }

        [Benchmark]
        [BenchmarkCategory(Count04)]
        public string StringConcat04() => string.Concat(_source00, _source01, _source02, _source03);

        [Benchmark]
        [BenchmarkCategory(Count08)]
        public string StringConcat08()
            => string.Concat(_source00, _source01, _source02, _source03, _source04, _source05, _source06, _source07);

        [Benchmark]
        [BenchmarkCategory(Count12)]
        public string StringConcat12()
            => string.Concat(_source00, _source01, _source02, _source03, _source04, _source05, _source06, _source07, _source08, _source09, _source10, _source11);

        [Benchmark]
        [BenchmarkCategory(Count04)]
        public string StringBuilder04()
        {
            var length = _source00.Length + _source01.Length + _source03.Length + _source04.Length;

            var sb = new StringBuilder(length);
            sb.Append(_source00);
            sb.Append(_source01);
            sb.Append(_source02);
            sb.Append(_source03);

            return sb.ToString();
        }

        [Benchmark]
        [BenchmarkCategory(Count08)]
        public string StringBuilder08()
        {
            var length = _source00.Length + _source01.Length + _source02.Length + _source03.Length +
                         _source04.Length + _source05.Length + _source06.Length + _source07.Length;

            var sb = new StringBuilder(length);
            sb.Append(_source00);
            sb.Append(_source01);
            sb.Append(_source02);
            sb.Append(_source03);
            sb.Append(_source04);
            sb.Append(_source05);
            sb.Append(_source06);
            sb.Append(_source07);

            return sb.ToString();
        }

        [Benchmark]
        [BenchmarkCategory(Count12)]
        public string StringBuilder12()
        {
            var length = _source00.Length + _source01.Length + _source02.Length + _source03.Length +
                         _source04.Length + _source05.Length + _source06.Length + _source07.Length +
                         _source08.Length + _source09.Length + _source10.Length + _source11.Length;

            var sb = new StringBuilder(length);
            sb.Append(_source00);
            sb.Append(_source01);
            sb.Append(_source02);
            sb.Append(_source03);
            sb.Append(_source04);
            sb.Append(_source05);
            sb.Append(_source06);
            sb.Append(_source07);
            sb.Append(_source08);
            sb.Append(_source09);
            sb.Append(_source10);
            sb.Append(_source11);

            return sb.ToString();
        }

        [Benchmark]
        [BenchmarkCategory(Count04)]
        public string Span04()
        {
            var length = _source00.Length + _source01.Length + _source03.Length + _source04.Length;

            var result = new string(default, length);
            var span = MemoryMarshal.CreateSpan(ref MemoryMarshal.GetReference(result.AsSpan()), length);

            _source00.AsSpan().CopyTo(span);
            var pos = _source00.Length;
            _source01.AsSpan().CopyTo(span[pos..]);
            pos += _source01.Length;
            _source02.AsSpan().CopyTo(span[pos..]);
            pos += _source02.Length;
            _source03.AsSpan().CopyTo(span[pos..]);

            return result;
        }

        [Benchmark]
        [BenchmarkCategory(Count08)]
        public string Span08()
        {
            var length = _source00.Length + _source01.Length + _source02.Length + _source03.Length +
                         _source04.Length + _source05.Length + _source06.Length + _source07.Length;

            var result = new string(default, length);
            var span = MemoryMarshal.CreateSpan(ref MemoryMarshal.GetReference(result.AsSpan()), length);

            _source00.AsSpan().CopyTo(span);
            var pos = _source00.Length;
            _source01.AsSpan().CopyTo(span[pos..]);
            pos += _source01.Length;
            _source02.AsSpan().CopyTo(span[pos..]);
            pos += _source02.Length;
            _source03.AsSpan().CopyTo(span[pos..]);
            pos += _source03.Length;
            _source04.AsSpan().CopyTo(span[pos..]);
            pos += _source04.Length;
            _source05.AsSpan().CopyTo(span[pos..]);
            pos += _source05.Length;
            _source06.AsSpan().CopyTo(span[pos..]);
            pos += _source06.Length;
            _source07.AsSpan().CopyTo(span[pos..]);

            return result;
        }

        [Benchmark]
        [BenchmarkCategory(Count12)]
        public string Span12()
        {
            var length = _source00.Length + _source01.Length + _source02.Length + _source03.Length +
                         _source04.Length + _source05.Length + _source06.Length + _source07.Length +
                         _source08.Length + _source09.Length + _source10.Length + _source11.Length;

            var result = new string(default, length);
            var span = MemoryMarshal.CreateSpan(ref MemoryMarshal.GetReference(result.AsSpan()), length);

            _source00.AsSpan().CopyTo(span);
            var pos = _source00.Length;
            _source01.AsSpan().CopyTo(span[pos..]);
            pos += _source01.Length;
            _source02.AsSpan().CopyTo(span[pos..]);
            pos += _source02.Length;
            _source03.AsSpan().CopyTo(span[pos..]);
            pos += _source03.Length;
            _source04.AsSpan().CopyTo(span[pos..]);
            pos += _source04.Length;
            _source05.AsSpan().CopyTo(span[pos..]);
            pos += _source05.Length;
            _source06.AsSpan().CopyTo(span[pos..]);
            pos += _source06.Length;
            _source07.AsSpan().CopyTo(span[pos..]);
            pos += _source07.Length;
            _source08.AsSpan().CopyTo(span[pos..]);
            pos += _source08.Length;
            _source09.AsSpan().CopyTo(span[pos..]);
            pos += _source09.Length;
            _source10.AsSpan().CopyTo(span[pos..]);
            pos += _source10.Length;
            _source11.AsSpan().CopyTo(span[pos..]);

            return result;
        }

        [Benchmark]
        [BenchmarkCategory(Count04)]
        public string UnsafeCopyBlockUnaligned04()
        {
            var length = _source00.Length + _source01.Length + _source02.Length + _source03.Length;

            var result = new string(default, length);
            ref var resultStart = ref Unsafe.As<char, byte>(ref MemoryMarshal.GetReference(result.AsSpan()));

            var pos = _source00.Length * sizeof(char);
            BinaryHelper.Copy(_source00, ref resultStart, pos);

            var byteCount = _source01.Length * sizeof(char);
            BinaryHelper.Copy(_source01, ref Unsafe.Add(ref resultStart, pos), byteCount);
            pos += byteCount;

            byteCount = _source02.Length * sizeof(char);
            BinaryHelper.Copy(_source02, ref Unsafe.Add(ref resultStart, pos), byteCount);
            pos += byteCount;

            byteCount = _source03.Length * sizeof(char);
            BinaryHelper.Copy(_source03, ref Unsafe.Add(ref resultStart, pos), byteCount);

            return result;
        }

        [Benchmark]
        [BenchmarkCategory(Count08)]
        public string UnsafeCopyBlockUnaligned08()
        {
            var length = _source00.Length + _source01.Length + _source02.Length + _source03.Length +
                         _source04.Length + _source05.Length + _source06.Length + _source07.Length;

            var result = new string(default, length);
            ref var resultStart = ref Unsafe.As<char, byte>(ref MemoryMarshal.GetReference(result.AsSpan()));

            var pos = _source00.Length * sizeof(char);
            BinaryHelper.Copy(_source00, ref resultStart, pos);

            var byteCount = _source01.Length * sizeof(char);
            BinaryHelper.Copy(_source01, ref Unsafe.Add(ref resultStart, pos), byteCount);
            pos += byteCount;

            byteCount = _source02.Length * sizeof(char);
            BinaryHelper.Copy(_source02, ref Unsafe.Add(ref resultStart, pos), byteCount);
            pos += byteCount;

            byteCount = _source03.Length * sizeof(char);
            BinaryHelper.Copy(_source03, ref Unsafe.Add(ref resultStart, pos), byteCount);
            pos += byteCount;

            byteCount = _source04.Length * sizeof(char);
            BinaryHelper.Copy(_source04, ref Unsafe.Add(ref resultStart, pos), byteCount);
            pos += byteCount;

            byteCount = _source05.Length * sizeof(char);
            BinaryHelper.Copy(_source05, ref Unsafe.Add(ref resultStart, pos), byteCount);
            pos += byteCount;

            byteCount = _source06.Length * sizeof(char);
            BinaryHelper.Copy(_source06, ref Unsafe.Add(ref resultStart, pos), byteCount);
            pos += byteCount;

            byteCount = _source07.Length * sizeof(char);
            BinaryHelper.Copy(_source07, ref Unsafe.Add(ref resultStart, pos), byteCount);

            return result;
        }

        [Benchmark]
        [BenchmarkCategory(Count12)]
        public string UnsafeCopyBlockUnaligned12()
        {
            var length = _source00.Length + _source01.Length + _source02.Length + _source03.Length +
                         _source04.Length + _source05.Length + _source06.Length + _source07.Length +
                         _source08.Length + _source09.Length + _source10.Length + _source11.Length;

            var result = new string(default, length);
            ref var resultStart = ref Unsafe.As<char, byte>(ref MemoryMarshal.GetReference(result.AsSpan()));

            var pos = _source00.Length * sizeof(char);
            BinaryHelper.Copy(_source00, ref resultStart, pos);

            var byteCount = _source01.Length * sizeof(char);
            BinaryHelper.Copy(_source01, ref Unsafe.Add(ref resultStart, pos), byteCount);
            pos += byteCount;

            byteCount = _source02.Length * sizeof(char);
            BinaryHelper.Copy(_source02, ref Unsafe.Add(ref resultStart, pos), byteCount);
            pos += byteCount;

            byteCount = _source03.Length * sizeof(char);
            BinaryHelper.Copy(_source03, ref Unsafe.Add(ref resultStart, pos), byteCount);
            pos += byteCount;

            byteCount = _source04.Length * sizeof(char);
            BinaryHelper.Copy(_source04, ref Unsafe.Add(ref resultStart, pos), byteCount);
            pos += byteCount;

            byteCount = _source05.Length * sizeof(char);
            BinaryHelper.Copy(_source05, ref Unsafe.Add(ref resultStart, pos), byteCount);
            pos += byteCount;

            byteCount = _source06.Length * sizeof(char);
            BinaryHelper.Copy(_source06, ref Unsafe.Add(ref resultStart, pos), byteCount);
            pos += byteCount;

            byteCount = _source07.Length * sizeof(char);
            BinaryHelper.Copy(_source07, ref Unsafe.Add(ref resultStart, pos), byteCount);
            pos += byteCount;

            byteCount = _source08.Length * sizeof(char);
            BinaryHelper.Copy(_source08, ref Unsafe.Add(ref resultStart, pos), byteCount);
            pos += byteCount;

            byteCount = _source09.Length * sizeof(char);
            BinaryHelper.Copy(_source09, ref Unsafe.Add(ref resultStart, pos), byteCount);
            pos += byteCount;

            byteCount = _source10.Length * sizeof(char);
            BinaryHelper.Copy(_source10, ref Unsafe.Add(ref resultStart, pos), byteCount);
            pos += byteCount;

            byteCount = _source11.Length * sizeof(char);
            BinaryHelper.Copy(_source11, ref Unsafe.Add(ref resultStart, pos), byteCount);

            return result;
        }

        [Benchmark]
        [BenchmarkCategory(Count04)]
        public string CopyChar04A()
        {
            var length = _source00.Length + _source01.Length + _source02.Length + _source03.Length;

            var result = new string(default, length);
            ref var resultStart = ref MemoryMarshal.GetReference(result.AsSpan());

            ref var sourceStart = ref MemoryMarshal.GetReference(_source00.AsSpan());
            var pos = _source00.Length;
            BinaryHelper.Copy(ref sourceStart, ref resultStart, pos);

            sourceStart = ref MemoryMarshal.GetReference(_source01.AsSpan());
            var charCount = _source01.Length;
            BinaryHelper.Copy(ref sourceStart, ref Unsafe.Add(ref resultStart, pos), charCount);
            pos += charCount;

            sourceStart = ref MemoryMarshal.GetReference(_source02.AsSpan());
            charCount = _source02.Length;
            BinaryHelper.Copy(ref sourceStart, ref Unsafe.Add(ref resultStart, pos), charCount);
            pos += charCount;

            sourceStart = ref MemoryMarshal.GetReference(_source03.AsSpan());
            charCount = _source03.Length;
            BinaryHelper.Copy(ref sourceStart, ref Unsafe.Add(ref resultStart, pos), charCount);

            return result;
        }

        [Benchmark]
        [BenchmarkCategory(Count08)]
        public string CopyChar08A()
        {
            var length = _source00.Length + _source01.Length + _source02.Length + _source03.Length +
                         _source04.Length + _source05.Length + _source06.Length + _source07.Length;

            var result = new string(default, length);
            ref var resultStart = ref MemoryMarshal.GetReference(result.AsSpan());

            ref var sourceStart = ref MemoryMarshal.GetReference(_source00.AsSpan());
            var pos = _source00.Length;
            BinaryHelper.Copy(ref sourceStart, ref resultStart, pos);

            sourceStart = ref MemoryMarshal.GetReference(_source01.AsSpan());
            var charCount = _source01.Length;
            BinaryHelper.Copy(ref sourceStart, ref Unsafe.Add(ref resultStart, pos), charCount);
            pos += charCount;

            sourceStart = ref MemoryMarshal.GetReference(_source02.AsSpan());
            charCount = _source02.Length;
            BinaryHelper.Copy(ref sourceStart, ref Unsafe.Add(ref resultStart, pos), charCount);
            pos += charCount;

            sourceStart = ref MemoryMarshal.GetReference(_source03.AsSpan());
            charCount = _source03.Length;
            BinaryHelper.Copy(ref sourceStart, ref Unsafe.Add(ref resultStart, pos), charCount);
            pos += charCount;

            sourceStart = ref MemoryMarshal.GetReference(_source04.AsSpan());
            charCount = _source04.Length;
            BinaryHelper.Copy(ref sourceStart, ref Unsafe.Add(ref resultStart, pos), charCount);
            pos += charCount;

            sourceStart = ref MemoryMarshal.GetReference(_source05.AsSpan());
            charCount = _source05.Length;
            BinaryHelper.Copy(ref sourceStart, ref Unsafe.Add(ref resultStart, pos), charCount);
            pos += charCount;

            sourceStart = ref MemoryMarshal.GetReference(_source06.AsSpan());
            charCount = _source06.Length;
            BinaryHelper.Copy(ref sourceStart, ref Unsafe.Add(ref resultStart, pos), charCount);
            pos += charCount;

            sourceStart = ref MemoryMarshal.GetReference(_source07.AsSpan());
            charCount = _source07.Length;
            BinaryHelper.Copy(ref sourceStart, ref Unsafe.Add(ref resultStart, pos), charCount);

            return result;
        }

        [Benchmark]
        [BenchmarkCategory(Count12)]
        public string CopyChar12A()
        {
            var length = _source00.Length + _source01.Length + _source02.Length + _source03.Length +
                         _source04.Length + _source05.Length + _source06.Length + _source07.Length +
                         _source08.Length + _source09.Length + _source10.Length + _source11.Length;

            var result = new string(default, length);
            ref var resultStart = ref MemoryMarshal.GetReference(result.AsSpan());

            ref var sourceStart = ref MemoryMarshal.GetReference(_source00.AsSpan());
            var pos = _source00.Length;
            BinaryHelper.Copy(ref sourceStart, ref resultStart, pos);

            sourceStart = ref MemoryMarshal.GetReference(_source01.AsSpan());
            var charCount = _source01.Length;
            BinaryHelper.Copy(ref sourceStart, ref Unsafe.Add(ref resultStart, pos), charCount);
            pos += charCount;

            sourceStart = ref MemoryMarshal.GetReference(_source02.AsSpan());
            charCount = _source02.Length;
            BinaryHelper.Copy(ref sourceStart, ref Unsafe.Add(ref resultStart, pos), charCount);
            pos += charCount;

            sourceStart = ref MemoryMarshal.GetReference(_source03.AsSpan());
            charCount = _source03.Length;
            BinaryHelper.Copy(ref sourceStart, ref Unsafe.Add(ref resultStart, pos), charCount);
            pos += charCount;

            sourceStart = ref MemoryMarshal.GetReference(_source04.AsSpan());
            charCount = _source04.Length;
            BinaryHelper.Copy(ref sourceStart, ref Unsafe.Add(ref resultStart, pos), charCount);
            pos += charCount;

            sourceStart = ref MemoryMarshal.GetReference(_source05.AsSpan());
            charCount = _source05.Length;
            BinaryHelper.Copy(ref sourceStart, ref Unsafe.Add(ref resultStart, pos), charCount);
            pos += charCount;

            sourceStart = ref MemoryMarshal.GetReference(_source06.AsSpan());
            charCount = _source06.Length;
            BinaryHelper.Copy(ref sourceStart, ref Unsafe.Add(ref resultStart, pos), charCount);
            pos += charCount;

            sourceStart = ref MemoryMarshal.GetReference(_source07.AsSpan());
            charCount = _source07.Length;
            BinaryHelper.Copy(ref sourceStart, ref Unsafe.Add(ref resultStart, pos), charCount);
            pos += charCount;

            sourceStart = ref MemoryMarshal.GetReference(_source08.AsSpan());
            charCount = _source08.Length;
            BinaryHelper.Copy(ref sourceStart, ref Unsafe.Add(ref resultStart, pos), charCount);
            pos += charCount;

            sourceStart = ref MemoryMarshal.GetReference(_source09.AsSpan());
            charCount = _source09.Length;
            BinaryHelper.Copy(ref sourceStart, ref Unsafe.Add(ref resultStart, pos), charCount);
            pos += charCount;

            sourceStart = ref MemoryMarshal.GetReference(_source10.AsSpan());
            charCount = _source10.Length;
            BinaryHelper.Copy(ref sourceStart, ref Unsafe.Add(ref resultStart, pos), charCount);
            pos += charCount;

            sourceStart = ref MemoryMarshal.GetReference(_source11.AsSpan());
            charCount = _source11.Length;
            BinaryHelper.Copy(ref sourceStart, ref Unsafe.Add(ref resultStart, pos), charCount);

            return result;
        }

        [Benchmark]
        [BenchmarkCategory(Count04)]
        public string CopyChar04B()
        {
            var length = _source00.Length + _source01.Length + _source02.Length + _source03.Length;

            var result = new string(default, length);
            ref var resultStart = ref MemoryMarshal.GetReference(result.AsSpan());

            ref var sourceStart = ref MemoryMarshal.GetReference(_source00.AsSpan());
            var pos = _source00.Length;
            BinaryHelper.CopyChar(ref sourceStart, ref resultStart, pos);

            sourceStart = ref MemoryMarshal.GetReference(_source01.AsSpan());
            var charCount = _source01.Length;
            BinaryHelper.CopyChar(ref sourceStart, ref Unsafe.Add(ref resultStart, pos), charCount);
            pos += charCount;

            sourceStart = ref MemoryMarshal.GetReference(_source02.AsSpan());
            charCount = _source02.Length;
            BinaryHelper.CopyChar(ref sourceStart, ref Unsafe.Add(ref resultStart, pos), charCount);
            pos += charCount;

            sourceStart = ref MemoryMarshal.GetReference(_source03.AsSpan());
            charCount = _source03.Length;
            BinaryHelper.CopyChar(ref sourceStart, ref Unsafe.Add(ref resultStart, pos), charCount);

            return result;
        }

        [Benchmark]
        [BenchmarkCategory(Count08)]
        public string CopyChar08B()
        {
            var length = _source00.Length + _source01.Length + _source02.Length + _source03.Length +
                         _source04.Length + _source05.Length + _source06.Length + _source07.Length;

            var result = new string(default, length);
            ref var resultStart = ref MemoryMarshal.GetReference(result.AsSpan());

            ref var sourceStart = ref MemoryMarshal.GetReference(_source00.AsSpan());
            var pos = _source00.Length;
            BinaryHelper.CopyChar(ref sourceStart, ref resultStart, pos);

            sourceStart = ref MemoryMarshal.GetReference(_source01.AsSpan());
            var charCount = _source01.Length;
            BinaryHelper.CopyChar(ref sourceStart, ref Unsafe.Add(ref resultStart, pos), charCount);
            pos += charCount;

            sourceStart = ref MemoryMarshal.GetReference(_source02.AsSpan());
            charCount = _source02.Length;
            BinaryHelper.CopyChar(ref sourceStart, ref Unsafe.Add(ref resultStart, pos), charCount);
            pos += charCount;

            sourceStart = ref MemoryMarshal.GetReference(_source03.AsSpan());
            charCount = _source03.Length;
            BinaryHelper.CopyChar(ref sourceStart, ref Unsafe.Add(ref resultStart, pos), charCount);
            pos += charCount;

            sourceStart = ref MemoryMarshal.GetReference(_source04.AsSpan());
            charCount = _source04.Length;
            BinaryHelper.CopyChar(ref sourceStart, ref Unsafe.Add(ref resultStart, pos), charCount);
            pos += charCount;

            sourceStart = ref MemoryMarshal.GetReference(_source05.AsSpan());
            charCount = _source05.Length;
            BinaryHelper.CopyChar(ref sourceStart, ref Unsafe.Add(ref resultStart, pos), charCount);
            pos += charCount;

            sourceStart = ref MemoryMarshal.GetReference(_source06.AsSpan());
            charCount = _source06.Length;
            BinaryHelper.CopyChar(ref sourceStart, ref Unsafe.Add(ref resultStart, pos), charCount);
            pos += charCount;

            sourceStart = ref MemoryMarshal.GetReference(_source07.AsSpan());
            charCount = _source07.Length;
            BinaryHelper.CopyChar(ref sourceStart, ref Unsafe.Add(ref resultStart, pos), charCount);

            return result;
        }

        [Benchmark]
        [BenchmarkCategory(Count12)]
        public string CopyChar12B()
        {
            var length = _source00.Length + _source01.Length + _source02.Length + _source03.Length +
                         _source04.Length + _source05.Length + _source06.Length + _source07.Length +
                         _source08.Length + _source09.Length + _source10.Length + _source11.Length;

            var result = new string(default, length);
            ref var resultStart = ref MemoryMarshal.GetReference(result.AsSpan());

            ref var sourceStart = ref MemoryMarshal.GetReference(_source00.AsSpan());
            var pos = _source00.Length;
            BinaryHelper.CopyChar(ref sourceStart, ref resultStart, pos);

            sourceStart = ref MemoryMarshal.GetReference(_source01.AsSpan());
            var charCount = _source01.Length;
            BinaryHelper.CopyChar(ref sourceStart, ref Unsafe.Add(ref resultStart, pos), charCount);
            pos += charCount;

            sourceStart = ref MemoryMarshal.GetReference(_source02.AsSpan());
            charCount = _source02.Length;
            BinaryHelper.CopyChar(ref sourceStart, ref Unsafe.Add(ref resultStart, pos), charCount);
            pos += charCount;

            sourceStart = ref MemoryMarshal.GetReference(_source03.AsSpan());
            charCount = _source03.Length;
            BinaryHelper.CopyChar(ref sourceStart, ref Unsafe.Add(ref resultStart, pos), charCount);
            pos += charCount;

            sourceStart = ref MemoryMarshal.GetReference(_source04.AsSpan());
            charCount = _source04.Length;
            BinaryHelper.CopyChar(ref sourceStart, ref Unsafe.Add(ref resultStart, pos), charCount);
            pos += charCount;

            sourceStart = ref MemoryMarshal.GetReference(_source05.AsSpan());
            charCount = _source05.Length;
            BinaryHelper.CopyChar(ref sourceStart, ref Unsafe.Add(ref resultStart, pos), charCount);
            pos += charCount;

            sourceStart = ref MemoryMarshal.GetReference(_source06.AsSpan());
            charCount = _source06.Length;
            BinaryHelper.CopyChar(ref sourceStart, ref Unsafe.Add(ref resultStart, pos), charCount);
            pos += charCount;

            sourceStart = ref MemoryMarshal.GetReference(_source07.AsSpan());
            charCount = _source07.Length;
            BinaryHelper.CopyChar(ref sourceStart, ref Unsafe.Add(ref resultStart, pos), charCount);
            pos += charCount;

            sourceStart = ref MemoryMarshal.GetReference(_source08.AsSpan());
            charCount = _source08.Length;
            BinaryHelper.CopyChar(ref sourceStart, ref Unsafe.Add(ref resultStart, pos), charCount);
            pos += charCount;

            sourceStart = ref MemoryMarshal.GetReference(_source09.AsSpan());
            charCount = _source09.Length;
            BinaryHelper.CopyChar(ref sourceStart, ref Unsafe.Add(ref resultStart, pos), charCount);
            pos += charCount;

            sourceStart = ref MemoryMarshal.GetReference(_source10.AsSpan());
            charCount = _source10.Length;
            BinaryHelper.CopyChar(ref sourceStart, ref Unsafe.Add(ref resultStart, pos), charCount);
            pos += charCount;

            sourceStart = ref MemoryMarshal.GetReference(_source11.AsSpan());
            charCount = _source11.Length;
            BinaryHelper.CopyChar(ref sourceStart, ref Unsafe.Add(ref resultStart, pos), charCount);

            return result;
        }
    }
}