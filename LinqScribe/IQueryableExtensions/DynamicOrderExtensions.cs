using System.Linq.Expressions;
using static LinqScribe.ExpressionExtensions;
using static LinqScribe.Guard;

namespace LinqScribe.IQueryableExtensions;

public static class DynamicOrderExtensions
{
    /// <summary>
    /// Dynamically orders the elements of a sequence based on a property name specified at runtime.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">The sequence of elements to order.</param>
    /// <param name="columnToOrderWith">The name of the property to order by. This must match a public property of <typeparamref name="TSource"/>.</param>
    /// <param name="ascending">If true, the ordering is ascending; if false, descending.</param>
    /// <returns>An <see cref="IOrderedQueryable{TSource}"/> whose elements are sorted according to the specified property and direction.</returns>
    /// <exception cref="ArgumentException">Thrown if the specified property does not exist on the type <typeparamref name="TSource"/>.</exception>
    public static IOrderedQueryable<TSource> OrderBy<TSource>(this IQueryable<TSource> source, string columnToOrderWith)
    {
        return (IOrderedQueryable<TSource>)BuildOrderingCall(source, columnToOrderWith, nameof(Queryable.OrderBy));
    }
    
    /// <summary>
    /// Dynamically orders the elements of a sequence in descending order based on a property name specified at runtime.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">The sequence of elements to order.</param>
    /// <param name="columnToOrderWith">The name of the property to order by. This must match a public property of <typeparamref name="TSource"/>.</param>
    /// <returns>An <see cref="IOrderedQueryable{TSource}"/> whose elements are sorted in descending order based on the specified property.</returns>
    /// <exception cref="ArgumentException">Thrown if the specified property does not exist on the type <typeparamref name="TSource"/>.</exception>
    public static IOrderedQueryable<TSource> OrderByDescending<TSource>(this IQueryable<TSource> source, string columnToOrderWith)
    {
        return (IOrderedQueryable<TSource>)BuildOrderingCall(source, columnToOrderWith, nameof(Queryable.OrderByDescending));
    }

    /// <summary>
    /// Performs a subsequent ordering of the elements in a sequence in ascending order, based on a property name specified at runtime.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">An ordered sequence of elements to perform the secondary sort on.</param>
    /// <param name="column">The name of the property to use for the secondary ordering.</param>
    /// <returns>An <see cref="IOrderedQueryable{TSource}"/> whose elements are sorted based on the secondary key.</returns>
    /// <exception cref="ArgumentException">Thrown if the specified property does not exist on the type <typeparamref name="TSource"/>.</exception>
    public static IOrderedQueryable<TSource> ThenBy<TSource>(this IOrderedQueryable<TSource> source, string column)
    {
        return (IOrderedQueryable<TSource>)BuildOrderingCall(source, column, nameof(Queryable.ThenBy));
    }
    
    /// <summary>
    /// Performs a subsequent ordering of the elements in a sequence in descending order, based on a property name specified at runtime.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">An ordered sequence of elements to perform the secondary sort on.</param>
    /// <param name="column">The name of the property to use for the secondary ordering.</param>
    /// <returns>An <see cref="IOrderedQueryable{TSource}"/> whose elements are sorted in descending order based on the secondary key.</returns>
    /// <exception cref="ArgumentException">Thrown if the specified property does not exist on the type <typeparamref name="TSource"/>.</exception>
    public static IOrderedQueryable<TSource> ThenByDescending<TSource>(this IOrderedQueryable<TSource> source, string column)
    {
        return (IOrderedQueryable<TSource>)BuildOrderingCall(source, column, nameof(Queryable.ThenByDescending));
    }

    /// <summary>
    /// Constructs and executes a dynamic LINQ method call (OrderBy, ThenBy, etc.) based on the provided method name and property name.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements in the source query.</typeparam>
    /// <param name="source">The query to apply the ordering to.</param>
    /// <param name="propertyName">The name of the property to order by. Must exist on <typeparamref name="TSource"/>.</param>
    /// <param name="methodName">The name of the LINQ ordering method to call (e.g., "OrderBy", "ThenBy").</param>
    /// <returns>A new <see cref="IQueryable{TSource}"/> with the specified ordering applied.</returns>
    /// <exception cref="ArgumentException">Thrown if the specified property does not exist on the type <typeparamref name="TSource"/>.</exception>
    private static IQueryable<TSource> BuildOrderingCall<TSource>(IQueryable<TSource> source, string propertyName, string methodName)
    {
        var sourceType = typeof(TSource);
        EnsureExist(sourceType, propertyName);

        // x => x.Property
        var lambda = BuildSelectorLambda<TSource>(propertyName, out var propertyType);

        // Build the full method call expression for OrderBy / ThenBy
        MethodCallExpression resultExp = Expression.Call(
            typeof(Queryable),
            methodName,
            [sourceType, propertyType],
            source.Expression,
            Expression.Quote(lambda) // Quote wraps the lambda so it can be passed as an expression tree
        );

        // Use the provider to create the new query with ordering applied
        return source.Provider.CreateQuery<TSource>(resultExp);
    }
}