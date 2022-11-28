
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
    where T : class, new()
{
    [Type(typeof(string))]
    [Generic<string>] 
    public string Method() => string.Empty;

    //[Generic<T>] // Not allowed! Generic attributes must be fully constructed types.
    public T MethodT() => new T();
}