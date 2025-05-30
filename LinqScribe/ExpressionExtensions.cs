using System.Linq.Expressions;

namespace LinqScribe;

public static class ExpressionExtensions
{
    /// <summary>
    /// Builds an expression similar to: (TSource x) => x.PropertyName
    /// </summary>
    public static LambdaExpression BuildSelectorLambda<TSource>(string propertyName, out Type propertyType)
    {
        var type = typeof(TSource);

        // Get the property (case-insensitive)
        var property = type.GetPublicInstanceProperty(propertyName);

        propertyType = property!.PropertyType;
        
        // Create the parameter expression: "x"
        var parameter = Expression.Parameter(type, "x");

        // Access the property: "x.Property"
        var propertyAccess = Expression.MakeMemberAccess(parameter, property);

        // Final lambda: x => x.Property
        return Expression.Lambda(propertyAccess, parameter);
    }
    
    /// <summary>
    /// Builds an expression similar to: (TSource x) => (object)x.PropertyName
    /// </summary>
    public static Expression<Func<TSource, object>> BuildGenericSelectorLambda<TSource>(string propertyName)
    {
        var type = typeof(TSource);

        // Get the property (case-insensitive)
        var property = type.GetPublicInstanceProperty(propertyName);

        // Create the parameter expression: "x"
        var parameter = Expression.Parameter(type, "x");

        // Access the property: "x.Property"
        var propertyAccess = Expression.MakeMemberAccess(parameter, property!);

        // If value type, box to object so expression returns object
        Expression body = property!.PropertyType.IsValueType
            ? Expression.Convert(propertyAccess, typeof(object))
            : propertyAccess;

        // Final lambda: x => (object)x.Property
        return Expression.Lambda<Func<TSource, object>>(body, parameter);
    }
    
    /// <summary>
    /// Merges predicates into a single predicate delimited with ANDs (&&)
    /// </summary>
    /// <param name="predicates">A list of predicates</param>
    /// <returns>The combined predicate</returns>
    public static BinaryExpression JoinPredicates(this IEnumerable<BinaryExpression> predicates)
    {
        var predicateResult = predicates.First();
        return predicates
            .Skip(1)
            .Aggregate(predicateResult, (current, predicate) => Expression.AndAlso(current, predicate));
    }
}