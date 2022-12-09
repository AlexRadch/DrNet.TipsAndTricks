using System;

// Pyramid Slide Down
// https://www.codewars.com/kata/551f23362ff852e2ab000037
public class PyramidSlideDown
{
    public static int LongestSlideDown(int[][] pyramid)
    {
        for (var i = pyramid.Length - 2; i >= 0; --i)
        {
            var ts = pyramid[i];
            var bs = pyramid[i + 1];
            for (var j = 0; j < ts.Length; ++j)
                ts[j] += Math.Max(bs[j], bs[j + 1]);
        }
        return pyramid[0][0];
    }
}
