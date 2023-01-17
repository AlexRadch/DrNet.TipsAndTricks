// Make a spiral
// https://www.codewars.com/kata/534e01fbbb17187c7e0000c6
public class Spiralizor
{
    public static int[,] Spiralize(int size)
    {
        var result = new int[size, size];
        int x = 0, y = 0, dx = 1, dy = 0; // from start to right
        int len = size - 1; // line length
        for (var line = 0; line <= size; line++) // size + 1 lines
        {
            for (int l = 0; l < len; l++) // fill line
                (result[y, x], x, y) = (1, x + dx, y + dy);

            (dx, dy) = (-dy, dx); // rotate right

            if (line > 0 && line % 2 == 0) len -= 2; // next line length
            if (len == 0) len = 1; // last line length
        }
        return result;
    }
}
