using BenchmarkDotNet.Running;

namespace Benchmarks.CSharp;

class Program
{
    static void Main(string[] args)
        => BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);
}
