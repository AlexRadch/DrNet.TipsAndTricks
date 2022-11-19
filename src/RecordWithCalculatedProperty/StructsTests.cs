namespace Structs;

public class StructsTests
{
    public void Test()
    {
        Console.WriteLine();
        Console.WriteLine("--------- Structs ---------");
        Console.WriteLine();

        {
            Rectangle r;
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
            ReqRectangle r;
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
        }
    }
}
