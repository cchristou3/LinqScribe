using System.Linq.Expressions;

namespace LinqScribe;

public static class ExpressionExtensions
{
    /// <summary>
    /// Builds a strongly-typed expression that accesses a specified property on <typeparamref name="TSource"/>.
    /// The resulting expression is equivalent to: <c>(TSource x) => x.PropertyName</c>.
    /// </summary>
    /// <typeparam name="TSource">The type of the input parameter in the lambda expression.</typeparam>
    /// <param name="propertyName">The name of the property to access. The search is case-insensitive.</param>
    /// <param name="propertyType">
    /// When this method returns, contains the <see cref="Type"/> of the specified property if found; otherwise, undefined.
    /// </param>
    /// <returns>
    /// A <see cref="LambdaExpression"/> representing a lambda expression that accesses the specified property on an instance of <typeparamref name="TSource"/>.
    /// </returns>
    /// <exception cref="ArgumentException">Thrown if the specified property does not exist on <typeparamref name="TSource"/>.</exception>
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
    /// Builds a lambda expression that dynamically accesses a specified property of <typeparamref name="TSource"/>.  
    /// The resulting expression is equivalent to: <c>(TSource x) => (dynamic)x.PropertyName</c>.
    /// </summary>
    /// <typeparam name="TSource">The type of the parameter in the lambda expression.</typeparam>
    /// <param name="propertyName">
    /// The name of the property to access. The lookup is case-insensitive and must match a public instance property on <typeparamref name="TSource"/>.
    /// </param>
    /// <returns>
    /// An <see cref="Expression{Func{TSource, dynamic}}"/> representing a lambda that returns the value of the specified property as <c>dynamic</c>.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Thrown if the specified property does not exist on <typeparamref name="TSource"/>.
    /// </exception>
    public static Expression<Func<TSource, dynamic>> BuildGenericSelectorLambda<TSource>(string propertyName)
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

        // Final lambda: x => (dynamic)x.Property
        return Expression.Lambda<Func<TSource, dynamic>>(body, parameter);
    }
}