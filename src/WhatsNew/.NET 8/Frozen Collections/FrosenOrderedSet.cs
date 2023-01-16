using System.Collections.Frozen;

namespace Frozen_Collections;

/// <summary>
/// Provides a set of initialization methods for instances of the <see cref="FrozenOrderedSet{T}"/> class.
/// </summary>
/// <remarks>
/// Frozen collections are immutable and are optimized for situations where a collection
/// is created very infrequently but is used very frequently at runtime. They have a relatively high
/// cost to create but provide excellent lookup performance. Thus, these are ideal for cases
/// where a collection is created once, potentially at the startup of an application, and used throughout
/// the remainder of the life of the application. Frozen collections should only be initialized with
/// trusted input.
/// </remarks>
public static class FrozenOrderedSet
{
    /// <summary>Creates a <see cref="FrozenOrderedSet{T}"/> with the specified values.</summary>
    /// <param name="source">The values to use to populate the set.</param>
    /// <param name="comparer">The comparer implementation to use to compare values for equality. If null, <see cref="EqualityComparer{T}.Default"/> is used.</param>
    /// <param name="orderComparer">The comparer implementation to use to compare values for there order. If null, <see cref="Comparer{T}.Default"/> is used.</param>
    /// <typeparam name="T">The type of the values in the set.</typeparam>
    /// <remarks>If the same key appears multiple times in the input, the latter one in the sequence takes precedence.</remarks>
    /// <returns>A frozen ordered set.</returns>
    public static FrozenOrderedSet<T> ToFrozenOrderedSet<T>(this IEnumerable<T> source, IEqualityComparer<T>? comparer = null, IComparer<T>? orderComparer = null)
    {
        ArgumentNullException.ThrowIfNull(source);
        comparer ??= EqualityComparer<T>.Default;
        orderComparer ??= Comparer<T>.Default;

        // If the source is already frozen with the same comparer, it can simply be returned.
        if (source is FrozenOrderedSet<T> existing &&
            existing.Comparer.Equals(comparer) && existing.OrderComparer.Equals(orderComparer))
        {
            return existing;
        }

        // Ensure we have a HashSet<,> using the specified comparer such that all values
        // are non-null and unique according to that comparer.
        if (source is not HashSet<T> uniqueValues ||
            (uniqueValues.Count != 0 && !uniqueValues.Comparer.Equals(comparer)))
        {
            uniqueValues = new HashSet<T>(source, comparer);
        }

        // If the input was empty, simply return the empty frozen set singleton. The comparer is ignored.
        if (uniqueValues.Count == 0)
        {
            return FrozenOrderedSet<T>.Empty;
        }

        if (typeof(T).IsValueType)
        {
            // Optimize for value types when the default comparer is being used. In such a case, the implementation
            // may use EqualityComparer<T>.Default.Equals/GetHashCode directly, with generic specialization enabling
            // the Equals/GetHashCode methods to be devirtualized and possibly inlined.
            if (ReferenceEquals(comparer, EqualityComparer<T>.Default))
            {
                // In the specific case of Int32 keys, we can optimize further to reduce memory consumption by using
                // the underlying FrozenHashtable's Int32 index as the values themselves, avoiding the need to store the
                // same values yet again.
                return typeof(T) == typeof(int) ?
                    (FrozenSet<T>)(object)new Int32FrozenSet((HashSet<int>)(object)uniqueValues) :
                    new ValueTypeDefaultComparerFrozenSet<T>(uniqueValues);
            }
        }
        else if (typeof(T) == typeof(string))
        {
            // Null is rare as a value in the set and we don't optimize for it.  This enables the ordinal string
            // implementation to fast-path out on null inputs rather than having to accomodate null inputs.
            if (!uniqueValues.Contains(default!))
            {
                // If the value is a string and the comparer is known to provide ordinal (case-sensitive or case-insensitive) semantics,
                // we can use an implementation that's able to examine and optimize based on lengths and/or subsequences within those strings.
                if (ReferenceEquals(comparer, EqualityComparer<T>.Default) ||
                    ReferenceEquals(comparer, StringComparer.Ordinal) ||
                    ReferenceEquals(comparer, StringComparer.OrdinalIgnoreCase))
                {
                    HashSet<string> stringValues = (HashSet<string>)(object)uniqueValues;
                    IEqualityComparer<string> stringComparer = (IEqualityComparer<string>)(object)comparer;

                    FrozenSet<string> frozenSet =
                        LengthBucketsFrozenSet.TryCreateLengthBucketsFrozenSet(stringValues, stringComparer) ??
                        (FrozenSet<string>)new OrdinalStringFrozenSet(stringValues, stringComparer);

                    return (FrozenSet<T>)(object)frozenSet;
                }
            }
        }

        // No special-cases apply. Use the default frozen set.
        return new DefaultFrozenSet<T>(uniqueValues, comparer);
    }
}


/// <summary>Provides an immutable, read-only set optimized for fast lookup and enumeration.</summary>
/// <typeparam name="T">The type of the values in this set.</typeparam>
/// <remarks>
/// Frozen collections are immutable and are optimized for situations where a collection
/// is created very infrequently but is used very frequently at runtime. They have a relatively high
/// cost to create but provide excellent lookup performance. Thus, these are ideal for cases
/// where a collection is created once, potentially at the startup of an application, and used throughout
/// the remainder of the life of the application. Frozen collections should only be initialized with
/// trusted input.
/// </remarks>
public abstract class FrozenOrderedSet<T>: FrozenSet<T>
{
    /// <summary>Initialize the set.</summary>
    /// <param name="comparer">The comparer to use and to expose from <see cref="Comparer"/>.</param>
    /// <param name="orderComparer">The order comparer to order set <see cref="OrderComparer"/>.</param>
    protected FrozenOrderedSet(IEqualityComparer<T> comparer, IComparer<T> orderComparer) : base(comparer) => OrderComparer = orderComparer;

    /// <summary>Gets the order comparer used by this set.</summary>
    public IComparer<T> OrderComparer { get; }

    /// <summary>Gets an empty <see cref="FrozenSet{T}"/>.</summary>
    public static FrozenOrderedSet<T> Empty { get; } = new EmptyFrozenOrderedSet<T>();
}
