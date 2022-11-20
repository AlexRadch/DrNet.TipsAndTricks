using System.Diagnostics.CodeAnalysis;

namespace Structs;

//public record Rectangle(required int Width, required int Height) // Compiler Error
public record struct Rectangle(int Width, int Height)
{
    private int EvaluateArea() => Width * Height;
    public int Area => EvaluateArea();

    public Rectangle(): this(0, 0) { }
}

//public partial record ReqRectangle
//{
//    [SetsRequiredMembers]
//    public partial ReqRectangle(int Width, int Height); // Compiler error
//}

public partial record struct ReqRectangle
//(required int Width, required int Height)// Compiler error
//(int Width, int Height) //Can not set [SetsRequiredMembers] on constructor
{
    public required int Width { get; init; } //= Width; // Moved to manual constructor
    public required int Height { get; init; } //= Height; // Moved to manual constructor

    private int EvaluateArea() => Width * Height;
    public int Area => EvaluateArea();

    [SetsRequiredMembers]
    //public partial ReqRectangle(int Width, int Height); // Compiler error
    public ReqRectangle(int Width, int Height)
    {
        this.Width = Width;
        this.Height = Height;
    }

    public ReqRectangle() { }
}