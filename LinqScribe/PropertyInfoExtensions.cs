using System.Reflection;

namespace LinqScribe;

public static class PropertyInfoExtensions
{
    public static PropertyInfo? GetPublicInstanceProperty(this Type type, string propertyName)
    {
        return type.GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
    }
}