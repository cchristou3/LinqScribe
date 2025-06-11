using System.Linq.Expressions;
using System.Reflection;

namespace LinqScribe.IQueryableExtensions;

public static class DynamicFiltersExtensions
{
    public static IQueryable<TSource> FilterWith<TSource, TFilters>(this IQueryable<TSource> source, TFilters filter) where TFilters : class
    {
        var parameter = Expression.Parameter(typeof(TSource), "p");
        var predicate = BuildExpression(typeof(TSource), filter, parameter);
        var lambda = Expression.Lambda<Func<TSource, bool>>(predicate!, parameter);
        return source.Where(lambda);
    }
    
    private static BinaryExpression? BuildExpression<TFilters>(Type sourceType, TFilters filter, Expression parameter) where TFilters : class
    {
        var filterProperties = filter
            .GetType()
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .ToDictionary(k => k.Name);

        if (filterProperties.Count == 0)
            return null;
        
        var filterNames = filterProperties.Keys.ToHashSet();
        
        var sourceProperties = sourceType
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(x => filterNames.Contains(x.Name))
            .ToList();

        if (sourceProperties.Count == 0)
            return null;
        
        // "p" is going to be the parameter in our lambda expressions. E.g. (p => ...)
        BinaryExpression? predicate = null;
        foreach (var sourceProperty in sourceProperties)
        {
            object? value = filterProperties[sourceProperty.Name].GetValue(filter);

            if (value == null) continue;
            
            var propertyAccess = Expression.Property(parameter, sourceProperty); // Access the property: "x.SourceProperty"

            BinaryExpression? expression;
            if (IsPrimitiveOrString(sourceProperty.PropertyType))
            {
                expression = propertyAccess.EqualsTo(value);
            }
            else
            {
                expression = BuildExpression(sourceProperty.PropertyType, value, propertyAccess);
            }
            predicate = predicate == null ? expression : Expression.AndAlso(predicate, expression!);
        }

        return predicate;
    }
    
    private static bool IsPrimitiveOrString(Type type) =>
        type.IsPrimitive || type == typeof(string) || type == typeof(DateTime) || type == typeof(decimal);
}