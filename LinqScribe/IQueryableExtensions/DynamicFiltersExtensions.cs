using System.Linq.Expressions;
using System.Reflection;

namespace LinqScribe.IQueryableExtensions;

public static class DynamicFiltersExtensions
{
    public static IQueryable<TSource> FilterWith<TSource, TFilters>(this IQueryable<TSource> source, TFilters filter) where TFilters : class
    {
        var filterProperties = filter
            .GetType()
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .ToDictionary(k => k.Name);
        
        if (filterProperties.Count == 0)
            return source;
        
        var filterNames = filterProperties.Keys.ToHashSet();
        
        var sourceType = typeof(TSource);
        var sourceProperties = sourceType
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(x => filterNames.Contains(x.Name))
            .ToList();
        
        if (sourceProperties.Count == 0)
            return source;
        
        // "p" is going to be the parameter in our lambda expressions. E.g. (p => ...)
        var parameter = Expression.Parameter(sourceType, "p");
        BinaryExpression? predicate = null;
        foreach (var sourceProperty in sourceProperties)
        {
            object? filterValue = filterProperties[sourceProperty.Name].GetValue(filter);

            if (filterValue == null) continue;
            
            // Access the property: "x.SourceProperty"
            var propertyAccess = Expression.MakeMemberAccess(parameter, sourceProperty);

            // x.SourceProperty == filterValue
            if (predicate is null)
            {
                predicate = propertyAccess.EqualsTo(filterValue);
            }
            else
            {
                predicate = predicate.AndAlso(propertyAccess.EqualsTo(filterValue));
            }
        }
        
        var lambda = Expression.Lambda<Func<TSource, bool>>(predicate!, parameter);
        return source.Where(lambda);
    }
}