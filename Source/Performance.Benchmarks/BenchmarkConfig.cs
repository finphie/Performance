using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Jobs;

namespace Performance.Benchmarks;

public class BenchmarkConfig : ManualConfig
{
    public BenchmarkConfig()
    {
        AddExporter(MarkdownExporter.GitHub);
        AddLogicalGroupRules(BenchmarkLogicalGroupRule.ByCategory);
        AddColumn(CategoriesColumn.Default);
        AddDiagnoser(MemoryDiagnoser.Default);
        AddDiagnoser(new DisassemblyDiagnoser(new DisassemblyDiagnoserConfig(printSource: true)));

        AddJob(Job.Default.WithRuntime(CoreRuntime.Core60)
            .WithId("Default"));

        AddJob(Job.Default.WithRuntime(CoreRuntime.Core60)
            .WithEnvironmentVariables(
                new EnvironmentVariable("COMPlus_TieredCompilation", "0"))
            .WithId("NoTieredCompilation"));

        AddJob(Job.Default.WithRuntime(CoreRuntime.Core60)
            .WithEnvironmentVariables(
                new EnvironmentVariable("COMPlus_ReadyToRun", "0"),
                new EnvironmentVariable("COMPlus_TC_QuickJitForLoops", "1"),
                new EnvironmentVariable("COMPlus_TieredPGO", "0"))
            .WithId("NoReadyToRun, QuickJitForLoops"));

        AddJob(Job.Default.WithRuntime(CoreRuntime.Core60)
            .WithEnvironmentVariables(
                new EnvironmentVariable("COMPlus_ReadyToRun", "0"),
                new EnvironmentVariable("COMPlus_TC_QuickJitForLoops", "1"),
                new EnvironmentVariable("COMPlus_TieredPGO", "1"))
            .WithId("NoReadyToRun, QuickJitForLoops, TieredPGO"));
    }
}
