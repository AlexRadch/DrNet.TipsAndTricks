﻿using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;

public class Program
{
    public static void Main(string[] args)
    {
        var _ = BenchmarkRunner.Run<PrimesBench>(DefaultConfig.Instance
                .WithOptions(ConfigOptions.DisableOptimizationsValidator),
            args);
    }
}

