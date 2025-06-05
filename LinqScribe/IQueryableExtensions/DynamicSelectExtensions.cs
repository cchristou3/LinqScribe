using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using static LinqScribe.Guard;
using static LinqScribe.ExpressionExtensions;

namespace LinqScribe.IQueryableExtensions;

public static class DynamicSelectExtensions
{
    /// <summary>
    /// Selects the property of the given database model.
    /// <code>
    /// // SELECT Name FROM Entities
    /// context.Entities.Select("Name").ToList();
    /// </code>
    /// </summary>
    /// <param name="source">The IQueryable source to select the property from.</param>
    /// <param name="property">The property to select.</param>
    /// <typeparam name="TSource">Any database model.</typeparam>
    /// <returns>The IQueryable selecting the given property.</returns>
    public static IQueryable<dynamic> Select<TSource>(this IQueryable<TSource> source, string property) where TSource : class
    {
        EnsureExist(typeof(TSource), property);

        // x => x.Property
        var selectLambda = BuildGenericSelectorLambda<TSource>(property);

        return source.Select(selectLambda);
    }

    /// <summary>
    /// Selects the provided properties of the given database model.
    /// <code>
    /// // SELECT Name, Age FROM Entities
    /// context.Entities.Select("Name", "Age").ToList();
    /// </code>
    /// </summary>
    /// <param name="source">The IQueryable source to select the property from.</param>
    /// <param name="properties">The properties to select.</param>
    /// <typeparam name="TSource">Any database model.</typeparam>
    /// <returns>A projection of the IQueryable that contains the given properties.</returns>
    public static IQueryable<dynamic> Select<TSource>(this IQueryable<TSource> source, params string[] properties)
    {
        var type = typeof(TSource);
        EnsureExist(type, properties);

        // "p" is going to be the parameter in our lambda expressions. E.g. (p => ...)
        var parameter = Expression.Parameter(type, "p");

        // Create a list of MemberExpression for each property to retrieve
        var propertyAccessList = new Dictionary<string, MemberExpression>(properties.Length);
        var propertyTypes = new Dictionary<string, Type>(properties.Length);
        foreach (var propertyName in properties)
        {
            // Get the property (case-insensitive)
            var property = type.GetPublicInstanceProperty(propertyName);

            propertyAccessList[propertyName] = Expression.MakeMemberAccess(parameter, property!);
            propertyTypes[propertyName] = property!.PropertyType;
        }

        // Create the anonymous type dynamically
        var anonymousType = CreateAnonymousType(propertyTypes);

        // Create the NewExpression for the anonymous type:
        // new AnonymousType()
        var newExpression = Expression.New(anonymousType.GetConstructors().First());

        var bindings = propertyAccessList
            .Select(entry =>
            {
                var propertyName = entry.Key;
                var member = anonymousType.GetMember(propertyName);
                return Expression.Bind(member[0], entry.Value);
            });
        
        // Set properties of the anonymous type:
        // new AnonymousType() { Id = p.Id, Name = p.Name }
        var memberInit = Expression.MemberInit(newExpression, bindings);

        // Convert it to a lambda:
        // p => new AnonymousType() { Id = p.Id, Name = p.Name }
        var selectLambda = Expression.Lambda<Func<TSource, dynamic>>(memberInit, parameter);
        return source.Select(selectLambda);
    }

    /// <summary>
    /// Creates a new "Anonymous" type that contains the provided properties.
    /// </summary>
    /// <param name="properties">A dictionary containing the property names and their corresponding type.</param>
    /// <returns>An anonymous type containing the provided properties.</returns>
    private static Type CreateAnonymousType(Dictionary<string, Type> properties)
    {
        AssemblyName assemblyName = new AssemblyName("AnonymousTypes");
        AssemblyBuilder assemblyBuilder =
            AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
        ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule("MainModule");
        TypeBuilder typeBuilder =
            moduleBuilder.DefineType("AnonymousType", TypeAttributes.Public | TypeAttributes.Class);

        foreach (var entry in properties)
        {
            typeBuilder.DefineField(entry.Key, entry.Value, FieldAttributes.Public);
        }

        return typeBuilder.CreateType();
    }
}