using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Toolchains.InProcess.NoEmit;
using Benchmarks.TrieVsDictionary;

internal class Program
{
  private static void Main(string[] args)
  {
    var config = DefaultConfig.Instance
      .AddJob(Job
        .LongRun
        .WithLaunchCount(1)
        .WithToolchain(InProcessNoEmitToolchain.Instance)
      );
    var _ = BenchmarkRunner.Run<Benchmark>(config, args);
  }
}