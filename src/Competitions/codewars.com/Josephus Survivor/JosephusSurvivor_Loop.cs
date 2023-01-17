// Josephus Survivor
// https://www.codewars.com/kata/555624b601231dc7a400017a
//
// Loop
public class JosephusSurvivor_Loop
{
    public static int JosSurvivor(int n, int k)
    {
        var result = 0;
        for (var i = 2; i <= n; i++)
            result = (result + k) % i;
        return result + 1;
    }
}