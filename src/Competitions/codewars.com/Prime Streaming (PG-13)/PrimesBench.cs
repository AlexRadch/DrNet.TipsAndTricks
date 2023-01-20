using BenchmarkDotNet.Attributes;
using System.Linq;

[RPlotExporter]
public class PrimesBench
{
    [Params(100, 10_000, 1_000_000)]
    public int N;

    //[Benchmark]
    //public void List() => Primes_List.Stream().Take(N).Count();

    [Benchmark(Baseline = true)]
    public void Sieve() => Primes_Sieve.Stream().Take(N).Count();

    [Benchmark]
    public void SieveO() => Primes_SieveO.Stream().Take(N).Count();
}