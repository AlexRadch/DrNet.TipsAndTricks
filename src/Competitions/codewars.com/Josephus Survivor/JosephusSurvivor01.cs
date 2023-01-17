using System.Collections;

// Josephus Survivor
// https://www.codewars.com/kata/555624b601231dc7a400017a
public class JosephusSurvivor
{
    public static int JosSurvivor(int n, int k)
    {
        var aliveCount = n;
        var alive = new BitArray(aliveCount, true);

        var index = 0; var count = 0;
        while (true)
        {
            if (alive[index] && ++count == k)
                if (--aliveCount == 0)
                    return index + 1;
                else
                    (alive[index], count) = (false, 0);
            index = (index + 1) % n;
        }
    }
}