using System.Linq.Expressions;

namespace Api.GraphQL.Helpers;

public static class ExpressionExtensions
{
    public static Expression<Func<T, bool>> Concatenate<T>(
        Expression<Func<T, bool>> firstExpression,
        Expression<Func<T, bool>> secondExpression)
    {
        var invokedThird = Expression.Invoke(secondExpression, firstExpression.Parameters);
        var finalExpression = Expression.Lambda<Func<T, bool>>(Expression.AndAlso(firstExpression.Body, invokedThird),
            firstExpression.Parameters);
        return finalExpression;
    }
}