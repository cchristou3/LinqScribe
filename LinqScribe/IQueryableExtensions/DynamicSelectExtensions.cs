using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using static LinqScribe.Guard;
using static LinqScribe.ExpressionExtensions;

namespace LinqScribe.IQueryableExtensions;

public static class DynamicSelectExtensions
{
    /*
     * Concept:
     * this.CustomSet(tableWithChildType)
     *  .SelectProperty<string>(tableWithChildType, propertyToRetrieve: "Name")
     *  .FirstOrDefault();
     * This will translate to the following SQL:
     * SELECT Name FROM [tableWithChildType]
     */
    public static IQueryable<dynamic> Select<TSource>(this IQueryable<TSource> source, string propertyToRetrieve)
    {
        var type = typeof(TSource);
        EnsureExist(type, propertyToRetrieve);

        // x => x.Property
        var selectLambda = BuildGenericSelectorLambda<TSource>(propertyToRetrieve);

        return source.Select(selectLambda);

        // Create an expression that calls the Select method of Queryable with the above constructed expression lambda
        var resultExp = Expression.Call(
            typeof(Queryable),
            nameof(Queryable.Select),
            [type, typeof(object)],
            source.Expression,
            Expression.Quote(selectLambda)
        );

        return source.Provider.CreateQuery<dynamic>(resultExp);
    }

    /*
     * Concept:
     * this.CustomSet(tableWithChildType)
     *  .SelectProperties(tableWithChildType, propertiesToRetrieve: new[] { "Name", "Age", "Modules" })
     *  .FirstOrDefault();
     * This will translate to the following SQL:
     * SELECT Name, Age, Modules FROM [tableWithChildType]
     */
    public static IQueryable<dynamic> Select<TSource>(this IQueryable<TSource> source, params string[] propertiesToRetrieve)
    {
        var type = typeof(TSource);
        EnsureExist(type, propertiesToRetrieve);

        // "p" is going to be the parameter in our lambda expressions. E.g. (p => ...)
        var parameter = Expression.Parameter(type, "p");

        // Create a list of MemberExpression for each property to retrieve
        var propertyAccessList = new Dictionary<string, MemberExpression>(propertiesToRetrieve.Length);
        var propertyTypes = new Dictionary<string, Type>(propertiesToRetrieve.Length);
        foreach (var propertyName in propertiesToRetrieve)
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
        var selectLambda = Expression.Lambda(memberInit, parameter);

        // Create an expression that calls the Select method of Queryable with the above constructed expression lambda
        var resultExp = Expression.Call(
            typeof(Queryable),
            nameof(Queryable.Select),
            [type, anonymousType],
            source.Expression,
            Expression.Quote(selectLambda)
        );

        return source.Provider.CreateQuery<dynamic>(resultExp);
    }
    

    private static Type CreateAnonymousType(Dictionary<string, Type> propertyTypes)
    {
        AssemblyName assemblyName = new AssemblyName("AnonymousTypes");
        AssemblyBuilder assemblyBuilder =
            AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
        ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule("MainModule");
        TypeBuilder typeBuilder =
            moduleBuilder.DefineType("AnonymousType", TypeAttributes.Public | TypeAttributes.Class);

        foreach (var entry in propertyTypes)
        {
            typeBuilder.DefineField(entry.Key, entry.Value, FieldAttributes.Public);
        }

        return typeBuilder.CreateType();
    }
}