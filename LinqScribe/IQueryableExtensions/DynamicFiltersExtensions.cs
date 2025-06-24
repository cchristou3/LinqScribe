using System.Collections;
using System.Linq.Expressions;
using System.Reflection;

namespace LinqScribe.IQueryableExtensions;

public static class DynamicFiltersExtensions
{
    private const string ListPostfix = "List";
    
    public static IQueryable<TSource> FilterWith<TSource, TFilters>(this IQueryable<TSource> source, TFilters filter) where TFilters : class
    {
        var parameter = Expression.Parameter(typeof(TSource), "p");
        var predicate = BuildExpression(typeof(TSource), filter, parameter);
        if (predicate is null) return source;
        var lambda = Expression.Lambda<Func<TSource, bool>>(predicate, parameter);
        return source.Where(lambda);
    }
    
    private static Expression? BuildExpression<TFilters>(Type sourceType, TFilters filter, Expression parameter) where TFilters : class
    {
        var filterProperties = filter
            .GetType()
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .ToDictionary(k => RemoveListPostfixIfExists(k.Name));

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
        Expression? predicate = null;
        foreach (var sourceProperty in sourceProperties)
        {
            object? value = filterProperties[sourceProperty.Name].GetValue(filter);

            if (value == null) continue;
            
            var propertyAccess = Expression.Property(parameter, sourceProperty); // Access the property: "x.SourceProperty"

            Expression? boolExpression;
            if (IsValueTypeOrString(sourceProperty.PropertyType))
            {
                boolExpression = BuildComparisonExpression(value, filterProperties[sourceProperty.Name].PropertyType, propertyAccess);
            }
            else // For complex types (e.g., classes) iterate through its properties
            {
                boolExpression = BuildExpression(sourceProperty.PropertyType, value, propertyAccess);
            }
            predicate = predicate == null ? boolExpression : Expression.AndAlso(predicate, boolExpression!);
        }

        return predicate;
    }

    private static Expression BuildComparisonExpression(object? value, Type propertyType, MemberExpression propertyAccess)
    {
        if (value is not string && value is IEnumerable values)
        {
            // Build constant expression for the list
            var constantList = Expression.Constant(values);
            
            var containsMethod = propertyType
                .GetMethods()
                .FirstOrDefault(m => m.Name == nameof(Enumerable.Contains));
            
            if (containsMethod is null)
            {
                throw new InvalidOperationException($"Can't find `{nameof(Enumerable.Contains)}` method for {propertyType}");
            }

            // Build list.Contains(x.Property)
            return Expression.Call(constantList, containsMethod, propertyAccess);
        }
        
        // Build value == x.Property
        return propertyAccess.EqualsTo(value);
    }
    
    private static bool IsValueTypeOrString(Type type) =>
        type.IsValueType || type == typeof(string);
    
    private static string RemoveListPostfixIfExists(string name)
    {
        return name.Contains(ListPostfix) ? name.Replace(ListPostfix, string.Empty) : name;
    } 
}