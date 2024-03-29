﻿using System.Globalization;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using BenchmarkDotNet.Attributes;
using EnumsNET;

namespace Performance.Benchmarks;

/// <summary>
/// enumの値を取得
/// </summary>
/// <typeparam name="T">列挙型</typeparam>
[GenericTypeArguments(typeof(Test))]
public class EnumToIntBenchmark<T>
        where T : struct, Enum
{
    static readonly Func<T, int> ExpressionConvert;
    static readonly Func<T, int> ExpressionConvertChecked;

    static EnumToIntBenchmark()
    {
        var parameter = Expression.Parameter(typeof(T), null);

        ExpressionConvert = Expression
            .Lambda<Func<T, int>>(Expression.Convert(parameter, typeof(int)), parameter)
            .Compile();

        ExpressionConvertChecked = Expression
            .Lambda<Func<T, int>>(Expression.ConvertChecked(parameter, typeof(int)), parameter)
            .Compile();
    }

    [Benchmark]
    [Arguments(Test.A)]
    public int ConvertToInt32(T @enum) => Convert.ToInt32(@enum, CultureInfo.InvariantCulture);

    [Benchmark]
    [Arguments(Test.A)]
    public int AsCast(T @enum) => (int)(@enum as object);

    [Benchmark]
    [Arguments(Test.A)]
    public int DirectCast(T @enum) => (int)(object)@enum;

    [Benchmark]
    [Arguments(Test.A)]
    public int RefValue(T @enum) => (int)__refvalue(__makeref(@enum), Test);

    [Benchmark]
    [Arguments(Test.A)]
    public int UnsafeAs(T @enum) => Unsafe.As<T, int>(ref @enum);

    [Benchmark]
    [Arguments(Test.A)]
    public int LinqExpressionConvert(T @enum) => ExpressionConvert(@enum);

    [Benchmark]
    [Arguments(Test.A)]
    public int LinqExpressionConvertChecked(T @enum) => ExpressionConvertChecked(@enum);

    [Benchmark]
    [Arguments(Test.A)]
    public int EnumsNetToInt32(T @enum) => @enum.GetMember()!.ToInt32();

    [Benchmark]
    [Arguments(Test.A)]
    public int GetHashCode(T @enum) => @enum.GetHashCode();
}
