using System.Reflection;
using System.Runtime.CompilerServices;
using LinqScribe.Future;

namespace LinqScribe;

public static class Guard
{
    public static void EnsureValid(Type type, Condition condition) => EnsureExist(type, condition.PropertyName);

    public static void EnsureExist(Type type, string propertyName)
    {
        if (type.GetPublicInstanceProperty(propertyName) is null)
            throw new ArgumentException(
                $"Type [{type.Name}] does not have the following property [{propertyName}].");
    }

    public static void EnsureExist(Type type, params string[] propertyNames)
    {
        foreach (var propertyName in propertyNames)
        {
            EnsureExist(type, propertyName);
        }
    }
}