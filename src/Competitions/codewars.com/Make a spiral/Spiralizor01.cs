// Make a spiral
// https://www.codewars.com/kata/534e01fbbb17187c7e0000c6
public class Spiralizor
{
    public static int[,] Spiralize(int size)
    {
        var result = new int[size, size];
        bool InSquare(int x) => x >= 0 && x < size;
        bool IsFree(int x, int y) => !InSquare(x) || !InSquare(y) || result[y, x] == 0;

        var (x, y) = (0, 0);
        var (dx, dy) = (1, 0);
        var steps = 1;
        while (InSquare(x) && InSquare(y) && IsFree(x + dx, y + dy))
        {
            result[y, x] = 1;
            x += dx; y += dy;
            steps++;
        }
        x -= dx; y -= dy;
        (dx, dy) = (-dy, dx);

        while (steps >= 2)
        {
            steps = 0;
            x += dx; y += dy;

            while (InSquare(x) && InSquare(y) && IsFree(x + dx, y + dy))
            {
                result[y, x] = 1;
                x += dx; y += dy;
                steps++;
            }
            x -= dx; y -= dy;
            (dx, dy) = (-dy, dx);
        }

        return result;
    }
}
