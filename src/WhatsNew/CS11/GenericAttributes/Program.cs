
// Before C# 11:
public class TypeAttribute : Attribute
{
    public TypeAttribute(Type t) => MyType = t;

    public Type MyType { get; }
}

// C# 11 feature:
public class GenericAttribute<T> : Attribute
    where T : class
{
}


public class GenericClass<T>
    where T : class
{
    [Type(typeof(string))]
    [Generic<string>] 
    //[Generic<T>] // Not allowed! Generic attributes must be fully constructed types.
    public string Method() => string.Empty;
}