namespace CachedRecords;

public class CachedRecordsTests
{
    public void Test()
    {
        Console.WriteLine();
        Console.WriteLine("--------- CachedRecords ---------");
        Console.WriteLine();
        {
            Console.ForegroundColor = ConsoleColor.Red;

            CachedRectangle r;
            r = new(); // ???
            Console.WriteLine(r);
            r = new(10, 20);
            Console.WriteLine(r);
            r = new() { Width = 20, Height = 40 };
            Console.WriteLine(r);
            r = r with { Width = r.Height - 5, Height = r.Width + 5 };
            Console.WriteLine(r);
            //r = r with {Area = 123};
            //Console.WriteLine(r);
            Console.WriteLine();

            Console.ResetColor();
        }
        {
            CachedRectangleNoClone r;
            //r = new(); // ???
            //Console.WriteLine(r);
            r = new(10, 20);
            Console.WriteLine(r);
            //r = new() { Width = 20, Height = 40 };
            //Console.WriteLine(r);
            //r = r with { Width = r.Height - 5, Height = r.Width + 5 };
            //Console.WriteLine(r);
            //r = r with {Area = 123};
            //Console.WriteLine(r);
            Console.WriteLine();
        }
        {
            Console.ForegroundColor = ConsoleColor.Cyan;

            CachedRectangleNoCloneCtor r;
            //r = new(); // ???
            //Console.WriteLine(r);
            r = new(10, 20);
            Console.WriteLine(r);
            //r = new() { Width = 20, Height = 40 };
            //Console.WriteLine(r);
            //r = r with { Width = r.Height - 5, Height = r.Width + 5 };
            //Console.WriteLine(r);
            //r = r with {Area = 123};
            //Console.WriteLine(r);
            Console.WriteLine();

            Console.ResetColor();
        }
        {
            CachedRectangleOnInit r;
            r = new(); // ???
            Console.WriteLine(r);
            r = new(10, 20);
            Console.WriteLine(r);
            r = new() { Width = 20, Height = 40 };
            Console.WriteLine(r);
            r = r with { Width = r.Height - 5, Height = r.Width + 5 };
            Console.WriteLine(r);
            //r = r with {Area = 123};
            //Console.WriteLine(r);
            Console.WriteLine();
        }
        {
            Console.ForegroundColor = ConsoleColor.Green;

            CachedRectangleOnInitCtor r;
            //r = new(); // ???
            //Console.WriteLine(r);
            r = new(10, 20);
            Console.WriteLine(r);
            r = new() { Width = 20, Height = 40 };
            Console.WriteLine(r);
            r = r with { Width = r.Height - 5, Height = r.Width + 5 };
            Console.WriteLine(r);
            //r = r with {Area = 123};
            //Console.WriteLine(r);
            Console.WriteLine();

            Console.ResetColor();
        }
    }
}
