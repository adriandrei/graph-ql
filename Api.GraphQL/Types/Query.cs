using System.Linq.Expressions;
using Api.GraphQL.Data;
using Api.GraphQL.Data.Entities;
using Api.GraphQL.Helpers;
using Api.GraphQL.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;

namespace Api.GraphQL.Types;

public class Query
{
    [UsePaging(IncludeTotalCount = true)]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<UserModel> Users(
        [Service] ApiDbContext dbContext,
        [Service] IMapper mapper,
        string? nameSearchKey = null)
    {
        return dbContext.Users
            .Where(CreateUserQuery(nameSearchKey))
            .AsQueryable()
            .ProjectTo<UserModel>(mapper.ConfigurationProvider);
    }

    private static Expression<Func<User, bool>> CreateUserQuery(
        string? nameSearchKey = null)
    {
        Expression<Func<User, bool>> query = user => true;

        if (nameSearchKey != null)
            query = ExpressionExtensions.Concatenate(query, entity => entity.Name.Contains(nameSearchKey));

        return query;
    }
}