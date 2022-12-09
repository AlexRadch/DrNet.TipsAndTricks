
// Who likes it?
// https://www.codewars.com/kata/5266876b8f4bf2da9b000362
public static class Kata
{
    public static string Likes(string[] name) => name.Length switch
    {
        0 => "no one likes this",
        1 => $"{name[0]} likes this",
        2 => $"{name[0]} and {name[1]} like this",
        3 => $"{name[0]}, {name[1]} and {name[2]} like this",
        _ => $"{name[0]}, {name[1]} and {name.Length - 2} others like this",
    };
}