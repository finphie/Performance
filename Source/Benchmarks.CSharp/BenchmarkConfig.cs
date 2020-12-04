using System.Diagnostics.CodeAnalysis;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Jobs;

namespace Benchmarks.CSharp
{
    [SuppressMessage("Performance", "CA1812:Avoid uninstantiated internal classes", Justification = "BenchmarkDotNetで使用")]
    class BenchmarkConfig : ManualConfig
    {
        public BenchmarkConfig()
        {
            AddExporter(MarkdownExporter.GitHub);
            AddLogicalGroupRules(BenchmarkLogicalGroupRule.ByCategory);
            AddColumn(CategoriesColumn.Default);
            AddDiagnoser(MemoryDiagnoser.Default);
            AddDiagnoser(new DisassemblyDiagnoser(new DisassemblyDiagnoserConfig(printSource: true)));

            AddJob(Job.Default.WithRuntime(CoreRuntime.Core50)
                .WithId("Default"));

            AddJob(Job.Default.WithRuntime(CoreRuntime.Core50)
                .WithEnvironmentVariables(
                    new EnvironmentVariable("COMPlus_TieredCompilation", "0"))
                .WithId("NoTieredCompilation"));

            AddJob(Job.Default.WithRuntime(CoreRuntime.Core50)
                .WithEnvironmentVariables(
                    new EnvironmentVariable("COMPlus_ReadyToRun", "0"),
                    new EnvironmentVariable("COMPlus_TC_QuickJitForLoops", "1"),
                    new EnvironmentVariable("COMPlus_TieredPGO", "0"))
                .WithId("NoReadyToRun, QuickJitForLoops"));

            AddJob(Job.Default.WithRuntime(CoreRuntime.Core50)
                .WithEnvironmentVariables(
                    new EnvironmentVariable("COMPlus_ReadyToRun", "0"),
                    new EnvironmentVariable("COMPlus_TC_QuickJitForLoops", "1"),
                    new EnvironmentVariable("COMPlus_TieredPGO", "1"))
                .WithId("NoReadyToRun, QuickJitForLoops, TieredPGO"));
        }
    }
}