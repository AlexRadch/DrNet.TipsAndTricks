using System.Diagnostics.CodeAnalysis;

namespace CachedRecords;

public record CachedRectangle(int Width, int Height)
{
    private static int EvaluateArea(int Width, int Height) => Width * Height;
    //private int EvaluateArea() => EvaluateArea(Width, Height); // Not used

    public int Area { get; }
        //= EvaluateArea(); // Compiler error
        = EvaluateArea(Width, Height); // Wrong code?

    public CachedRectangle() : this(0, 0) { }
}

public record CachedRectangleNoClone(int Width, int Height)
{
    public int Width { get; } = Width;
    public int Height { get; } = Height;

    private static int EvaluateArea(int Width, int Height) => Width * Height;
    //private int EvaluateArea() => EvaluateArea(Width, Height); // Not used

    public int Area { get; }
        //= EvaluateArea(); // Compiler error
        = EvaluateArea(Width, Height); // Wrong code?
}

public record CachedRectangleNoCloneCtor//(int Width, int Height) // Compiler error. Moved to manual constructor
{
    public int Width { get; private set; } //= Width; // Moved to manual constructor
    public int Height { get; private set; } //= Height; // Moved to manual constructor

    private int EvaluateArea() => Width * Height;

    public int Area { get; private set; } //= EvaluateArea(); // Moved to manual constructor

    public CachedRectangleNoCloneCtor(int Width, int Height)
    {
        this.Width = Width;
        this.Height = Height;
        Area = EvaluateArea();
    }
}

public record CachedRectangleOnInit(int Width, int Height)
{
    private readonly int _width = Width; // Work around compiler error
    public int Width
    {
        get => _width;
        init { _width = value; Area = EvaluateArea(); }
    } //= Width; // Compiler error see https://github.com/dotnet/csharplang/discussions/6641

    private readonly int _height = Height; // Work around compiler error
    public int Height
    {
        get => _height;
        init { _height = value; Area = EvaluateArea(); }
    } //= Height; // Compiler error see https://github.com/dotnet/csharplang/discussions/6641

    private static int EvaluateArea(int Width, int Height) => Width * Height;
    private int EvaluateArea() => EvaluateArea(Width, Height);

    public int Area { get; private set; }
        //= EvaluateArea(); // Compiler error
        = EvaluateArea(Width, Height); // Wrong code?

    public CachedRectangleOnInit() : this(0, 0) { }
}

public record CachedRectangleOnInitCtor//(int Width, int Height) // Compiler error. Moved to manual constructor
{
    private readonly int _width;
    public required int Width
    {
        get => _width;
        init { _width = value; Area = EvaluateArea(); }
    } //= Width; // Moved to manual constructor

    private readonly int _height;
    public required int Height
    {
        get => _height;
        init { _height = value; Area = EvaluateArea(); }
    } //= Height; // Moved to manual constructor

    private int EvaluateArea() => Width * Height;

    public int Area { get; private set; }

    public CachedRectangleOnInitCtor() { }

    [SetsRequiredMembers]
    public CachedRectangleOnInitCtor(int Width, int Height)
    {
        _width = Width;
        _height = Height;
        Area = EvaluateArea();
    }
}