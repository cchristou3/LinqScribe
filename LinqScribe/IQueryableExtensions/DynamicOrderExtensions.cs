using System.Linq.Expressions;
using static LinqScribe.ExpressionExtensions;
using static LinqScribe.Guard;

namespace LinqScribe.IQueryableExtensions;

public static class DynamicOrderExtensions
{
    public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, string columnToOrderWith, bool ascending = true)
    {
        // Call the shared method with the appropriate method name for ordering
        return (IOrderedQueryable<T>)BuildOrderingCall(source, columnToOrderWith, ascending ? nameof(Queryable.OrderBy) : nameof(Queryable.OrderByDescending));
    }

    public static IOrderedQueryable<T> ThenBy<T>(this IOrderedQueryable<T> source, string column, bool ascending = true)
    {
        // Call the shared method with the appropriate method name for then-ordering
        return (IOrderedQueryable<T>)BuildOrderingCall(source, column, ascending ? nameof(Queryable.ThenBy) : nameof(Queryable.ThenByDescending));
    }

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