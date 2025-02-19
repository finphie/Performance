﻿using System.Text;

namespace Performance.Benchmarks.Helpers;

/// <summary>
/// <see cref="string"/>のヘルパークラス
/// </summary>
static class StringHelper
{
    /// <summary>
    /// UTF-16文字列を作成します。
    /// </summary>
    /// <param name="length">文字列の長さ</param>
    /// <returns>UTF-16文字列を返します。</returns>
    public static string CreateUtf16String(int length)
    {
        // UTF-16文字列作成
        static string CreateUtf16String() => Guid.NewGuid().ToString("N");

        var sb = new StringBuilder(64);
        sb.Append(CreateUtf16String());

        var count = length / sb.Length;
        for (var i = 0; i < count; i++)
        {
            sb.Append(CreateUtf16String());
        }

        return sb.ToString()[..length];
    }

    /// <summary>
    /// UTF-16文字列の配列を作成します。
    /// </summary>
    /// <param name="count">文字列の個数</param>
    /// <param name="length">文字列の長さ</param>
    /// <returns>UTF-16文字列の配列を返します。</returns>
    public static string[] CreateUtf16Strings(int count, int length)
        => [.. Enumerable.Range(1, count).Select(_ => CreateUtf16String(length))];
}
