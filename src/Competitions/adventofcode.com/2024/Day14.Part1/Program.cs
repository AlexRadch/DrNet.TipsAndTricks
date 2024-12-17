//  --- Day 14: Restroom Redoubt ---
//  https://adventofcode.com/2024/day/14

using System.Text.RegularExpressions;

using Robot = (int X, int Y, int DX, int DY);
using Point = (int X, int Y);

{
    var robots = File.ReadLines("input1.txt")
    .Select(ReadRobot)
    .ToArray().AsReadOnly();

var result = Solve(robots, 11, 7, 100);

Console.WriteLine(result);
}

{
var robots = File.ReadLines("input2.txt")
    .Select(ReadRobot)
    .ToArray().AsReadOnly();

var result = Solve(robots, 101, 103, 100);

Console.WriteLine(result);
}

static Robot ReadRobot(string line)
{
var matches = SignIntRegex().Matches(line);

return new Robot(
    int.Parse(matches[0].Value), int.Parse(matches[1].Value),
    int.Parse(matches[2].Value), int.Parse(matches[3].Value));
}

static int Solve<TRobots>(TRobots robots, int wide, int tall, int seconds) where TRobots: IEnumerable<Robot>
{
    var points = robots.Select(robot => Evaluate(robot, wide, tall, seconds));
    var quadrants = points.GroupBy(position => Quadrant(position.X, position.Y, wide, tall)); //.ToArray();
    quadrants = quadrants.Where(group => group.Key >= 0); //.ToArray();
    var counts = quadrants.Select(group => group.Count());
    var result = counts.Aggregate(1, (result, count) => result * count);
    return result;
}

static Point Evaluate(Robot robot, int wide, int tall, int seconds) =>
    checked(
        (((robot.X + robot.DX * seconds) % wide + wide) % wide,
        ((robot.Y + robot.DY * seconds) % tall + tall) % tall)
    );

static int Quadrant(int X, int Y, int wide, int tall) =>
    X == (wide / 2) || Y == (tall / 2)
    ? -1
    : X / (wide / 2 + 1) + Y / (tall / 2 + 1) * 2;

partial class Program
{
    [GeneratedRegex(@"-?\d+")]
    private static partial Regex SignIntRegex();
}
