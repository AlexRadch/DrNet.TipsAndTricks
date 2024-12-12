//  --- Day 12: Garden Groups ---
//  https://adventofcode.com/2024/day/12


{
    var map = File.ReadLines("input1.txt").ToArray();
    var result = Solve(map);
    Console.WriteLine($"{result}");
}

{
    var map = File.ReadLines("input2.txt").ToArray();
    var result = Solve(map);
    Console.WriteLine($"{result}");
}

{
    var map = File.ReadLines("input3.txt").ToArray();
    var result = Solve(map);
    Console.WriteLine($"{result}");
}

static long Solve<TMap>(TMap map) where TMap : IReadOnlyList<string>
{
    var height = map.Count;
    var width = map[0].Length;

    var regions = new List<Region>();
    var regionsMap = new Region?[height * width];

    for (var y = 0; y < height; y++)
        for (var x = 0; x < width; x++)
            AddArea(x, y);

    var costs = regions.Select(r => (long)r.Area * r.Perimeter);
    var result = costs.Sum();
    return result;

    void AddArea(int x, int y)
    {
        var region = regionsMap[y * width + x];
        if (region is not null)
            return;
        region = new Region
        {
            Area = 1,
            Perimeter =
                GetPerimeter(x, y - 1) +
                GetPerimeter(x + 1, y) +
                GetPerimeter(x, y + 1) +
                GetPerimeter(x - 1, y)
        };
        regions.Add(region);
        regionsMap[y * width + x] = region;

        AddToRegion(x, y - 1);
        AddToRegion(x + 1, y);
        AddToRegion(x, y + 1);
        AddToRegion(x - 1, y);

        void AddToRegion(int x2, int y2)
        {
            if (GetPerimeter(x2, y2) > 0 || regionsMap[y2 * width + x2] is not null)
                return;

            regionsMap[y2 * width + x2] = region;
            region.Area++;
            region.Perimeter +=
                GetPerimeter(x2, y2 - 1) +
                GetPerimeter(x2 + 1, y2) +
                GetPerimeter(x2, y2 + 1) +
                GetPerimeter(x2 - 1, y2);

            AddToRegion(x2, y2 - 1);
            AddToRegion(x2 + 1, y2);
            AddToRegion(x2, y2 + 1);
            AddToRegion(x2 - 1, y2);
        }

        int GetPerimeter(int x2, int y2) =>
            (y2 < 0 || y2 >= height || x2 < 0 || x2 >= width || map[y][x] != map[y2][x2])
            ? 1 : 0;
    }
}


record class Region
{
    public int Area { get; set; }
    public int Perimeter { get; set; }
}
