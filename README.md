# Jasily

a library for C#.



## dev

### when use `throw new ArgumentNullException();`

1. NEVER throw NullReferenceException.
1. before create new object or complex value.
1. all path safely.

e.g.1:

``` cs
public static IReadOnlyList<T> AsReadOnly<T>([NotNull] this IList<T> list)
{
    if (list == null) throw new ArgumentNullException(nameof(list));
    return new ReadOnlyCollection<T>(list); // create new instance.
}
```

e.g.2

``` cs
public static void Write([NotNull] this Stream stream, [NotNull] byte[] buffer)
{
    if (stream == null) throw new ArgumentNullException(nameof(stream));
    if (buffer == null) throw new ArgumentNullException(nameof(buffer));
    stream.Write(buffer, 0, buffer.Length); // buffer.Length maybe throw NullReferenceException.
}
```

e.g.3

``` cs
public static IOrderedEnumerable<T> OrderBy<T>([NotNull] this IEnumerable<T> source, [NotNull] IComparer<T> comparer)
    => source.OrderBy(z => z, comparer);
    // source.OrderBy is extension method, so it will not throw NullReferenceException.
    // AND, source.OrderBy should check comparer nullable.
    // SO, we do not need to throw ArgumentNullException.
```

e.g.4

``` cs
public static IOrderedEnumerable<T> OrderBy<T>([NotNull] this IEnumerable<T> source, [NotNull] IComparer<T> comparer)
{
    if (source == null) throw new ArgumentNullException(nameof(source));
    if (WTF)
    {
    }
    else
    {
        source.OrderBy(z => z, comparer); // must crash even unreachable.
    }
}
```
