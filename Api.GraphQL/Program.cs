using Api.GraphQL.Data;
using Api.GraphQL.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Api.GraphQL;

public class Query
{
    [UsePaging(IncludeTotalCount = true)]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<User> UsersAsQueryable(
        [Service] ApiDbContext dbContext) => dbContext.Users.AsQueryable();


    [UsePaging(IncludeTotalCount = true)]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<User> UsersByName(
        string key,
        [Service] ApiDbContext dbContext)
    {
        var sw = Stopwatch.StartNew();
        var result = dbContext.Users.Where(t => t.Name.Contains(key)).AsQueryable();
        Console.WriteLine($"{nameof(UsersByName)}: {sw.ElapsedMilliseconds}");

        return result;
    }

    [UsePaging(IncludeTotalCount = true)]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public async Task<List<User>> UsersByName2(
        string key,
        [Service] ApiDbContext dbContext)
    {
        var sw = Stopwatch.StartNew();
        var result = await dbContext.Users.Where(t => t.Name.Contains(key)).ToListAsync();
        Console.WriteLine($"{nameof(UsersByName2)}: {sw.ElapsedMilliseconds}");

        return result;
        
    }
}

public class Program
{
    private const int EntitiesToAdd = 10;
    private const int NumberOfAdds = 10;

    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddDbContext<ApiDbContext>(
            options => options.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=graph-ql;Trusted_Connection=True;MultipleActiveResultSets=true"),
            ServiceLifetime.Transient);
        builder.Services.AddGraphQLServer()
            .AddQueryType<Query>()
            .AddFiltering()
            .AddSorting()
            .AddProjections();

        builder.Services.AddAuthorization();


        var app = builder.Build();

        var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
        var dbContext = scope.ServiceProvider.GetService<ApiDbContext>();
        dbContext!.Database.Migrate();

        AddUsers(dbContext);
        AddPosts(dbContext);
        app.MapGraphQL();
        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.Run();
    }

    private static void AddUsers(ApiDbContext dbContext)
    {
        for(int j = 0; j < NumberOfAdds; j++)
        {
            var toAdd = new List<User>();
            for (int i = 0; i < EntitiesToAdd; i++)
            {
                toAdd.Add(new User(Guid.NewGuid().ToString(), DateTimeOffset.UtcNow, $"{i}{i}"));
            }
            dbContext.Users.AddRange(toAdd);
            dbContext.SaveChanges();
        }
    }
    private static void AddPosts(ApiDbContext dbContext)
    {
        for (int j = 0; j < NumberOfAdds; j++)
        {
            var toAdd = new List<Post>();
            for (int i = 0; i < EntitiesToAdd; i++)
            {
                toAdd.Add(new Post(Guid.NewGuid().ToString(), DateTimeOffset.UtcNow, $"{i}{i}"));
            }
            dbContext.Posts.AddRange(toAdd);
            dbContext.SaveChanges();
        }
    }
}