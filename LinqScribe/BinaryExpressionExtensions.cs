using System.Linq.Expressions;

namespace LinqScribe;

public static class BinaryExpressionExtensions
{
    public static BinaryExpression EqualsTo<T>(this MemberExpression propertyAccess, T value)
    {
        // x.property == value
        return Expression.Equal(propertyAccess, Expression.Constant(value));
    }
    
    public static BinaryExpression AndAlso(this BinaryExpression left, BinaryExpression right)
    {
        // left && right
        return Expression.AndAlso(left, right);
    }
}