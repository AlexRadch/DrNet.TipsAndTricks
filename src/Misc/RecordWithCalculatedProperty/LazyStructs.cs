using System.Diagnostics.CodeAnalysis;

namespace LazyStructs;

public record struct LazyRectangle(int Width, int Height)
{
    private static int EvaluateArea(int Width, int Height) => Width * Height;
    //private int EvaluateArea() => EvaluateArea(Width, Height); // Not used

    //private Lazy<int> lazyArea = new(() => EvaluateArea()); // Compiler error
    private readonly Lazy<int> _lazyArea = new(() => EvaluateArea(Width, Height)); // Wrong code?
    public int Area => _lazyArea.Value;

    public LazyRectangle() : this(0, 0) { }
}

public record struct LazyRectangleCloneCtor(int Width, int Height)
{
    private static int EvaluateArea(int Width, int Height) => Width * Height;
    private int EvaluateArea() => EvaluateArea(Width, Height);

    //private readonly Lazy<int> _lazyArea = new(EvaluateArea); // Compiler error
    //private readonly Lazy<int> _lazyArea = new(() => EvaluateArea()); // Compiler error
    private readonly Lazy<int> _lazyArea = new(() => EvaluateArea(Width, Height)); // Wrong code?
    public int Area => _lazyArea.Value;

    public LazyRectangleCloneCtor()
        : this(0, 0) // Compiler required
    {
        _lazyArea = new(EvaluateArea); // Work around wrong code
    }

    //protected LazyRectangleCloneCtor(LazyRectangleCloneCtor original)
    ////: this(original.Width, original.Height) // Compiler error
    //{
    //    // Rewrited code from this(Width, Height) 
    //    Width = original.Width;
    //    Height = original.Height;
    //    _lazyArea = new(EvaluateArea);
    //}
}

public record struct LazyRectangle2Ctor//(int Width, int Height) // Moved to Ctor
{
    public required int Width { get; init; }
    public required int Height { get; init; }

    private int EvaluateArea() => Width * Height;

    private Lazy<int> _lazyArea;// = new(EvaluateArea); // Compiler error
    public int Area => _lazyArea.Value;

    public LazyRectangle2Ctor()
    {
        _lazyArea = new(EvaluateArea);
    }

    [SetsRequiredMembers]
    public LazyRectangle2Ctor(int Width, int Height)
    {
        this.Width = Width;
        this.Height = Height;
        _lazyArea = new(EvaluateArea);
    }

    //[SetsRequiredMembers]
    //protected LazyRectangle2Ctor(LazyRectangle2Ctor original)
    ////: this(original.Width, original.Height) // Compiler error
    //{
    //    // Rewrited code from this(Width, Height) 
    //    Width = original.Width;
    //    Height = original.Height;
    //    _lazyArea = new(EvaluateArea);
    //}
}
