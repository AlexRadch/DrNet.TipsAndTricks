using BenchmarkDotNet.Attributes;

[RPlotExporter]
public class JosephusSurvivorBench
{
    //[Params(100, 1000, 10_000)]
    [Params(2000)]
    public int N;

    //[Params(1, 3, 25, 99, 999)]
    [Params(25)]
    public int K;

    [Benchmark]
    public void SimpleCounting() => JosephusSurvivor_SimpleCounting.JosSurvivor(N, K);

    [Benchmark]
    public void ListCounting() => JosephusSurvivor_ListCounting.JosSurvivor(N, K);

    [Benchmark]
    public void LinkedListCounting() => JosephusSurvivor_LinkedListCounting.JosSurvivor(N, K);

    [Benchmark]
    public void Recursion() => JosephusSurvivor_Recursion.JosSurvivor(N, K);

    [Benchmark]
    public void Loop() => JosephusSurvivor_Loop.JosSurvivor(N, K);

    [Benchmark]
    public void RecursionO() => JosephusSurvivor_RecursionO.JosSurvivor(N, K);

    [Benchmark]
    public void RecursionOLoop() => JosephusSurvivor_RecursionOLoop.JosSurvivor(N, K);

}