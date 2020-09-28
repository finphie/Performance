#define Core30

using System.Diagnostics.CodeAnalysis;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Toolchains.CsProj;

#if CoreRt || CoreRtCpp
using BenchmarkDotNet.Environments;
#endif
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Jobs;
#if CoreRtCpp
using BenchmarkDotNet.Toolchains.CoreRt;
#endif

namespace Benchmarks.CSharp
{
    [SuppressMessage("Performance", "CA1812:Avoid uninstantiated internal classes", Justification = "BenchmarkDotNetで使用される")]
    class BenchmarkConfig : ManualConfig
    {
        public BenchmarkConfig()
        {
            Add(MarkdownExporter.GitHub);
            Add(MemoryDiagnoser.Default);
            Add(BenchmarkLogicalGroupRule.ByCategory);
            Add(CategoriesColumn.Default);
            Add(DisassemblyDiagnoser.Create(new DisassemblyDiagnoserConfig(printSource: true)));

#if Core30
            Add(Job.Default.With(CsProjCoreToolchain.NetCoreApp30));
#endif
#if CoreRt
            // CoreRT（RyuJIT利用）
            // cf. https://benchmarkdotnet.org/articles/configs/toolchains.html
            // cf. https://github.com/dotnet/corert/blob/master/Documentation/how-to-build-and-run-ilcompiler-in-console-shell-prompt.md
            Add(Job.Default.With(CoreRtRuntime.CoreRt30));
#endif
#if CoreRtCpp
            // CoreRT（CPP Code Generator利用）
            Add(Job.Default
                .With(CoreRtToolchain.CreateBuilder()
                    .UseCoreRtLocal(@"\corert\bin\Windows_NT.x64.Release")
                    .UseCppCodeGenerator()
                    .TargetFrameworkMoniker("netcoreapp3.0")
                    .ToToolchain()));
#endif
        }
    }
}