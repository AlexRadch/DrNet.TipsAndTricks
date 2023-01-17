// Josephus Survivor
// https://www.codewars.com/kata/555624b601231dc7a400017a
//
// Recursion
public class JosephusSurvivor_Recursion
{
    public static int JosSurvivor(int n, int k)
        => JosSurvivor0(n, k) + 1;

    private static int JosSurvivor0(int n, int k) => n switch
    {
        1 => 0,
        _ => (JosSurvivor0(n - 1, k) + k) % n,
    };
}