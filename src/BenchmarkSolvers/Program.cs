using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using GeniusSquareWeb.SolverBenchmark;

var config =
    DefaultConfig.Instance.AddColumn(
        StatisticColumn.P50,
        StatisticColumn.P95);
var summary = BenchmarkRunner.Run<FindOneSolutionBenchmark>(config);
