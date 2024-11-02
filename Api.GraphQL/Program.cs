using Api.GraphQL.Data;
using Api.GraphQL.Data.Entities;
using Api.GraphQL.Types;
using Microsoft.EntityFrameworkCore;

namespace Api.GraphQL;

public class Program
{
    private const int EntitiesToAdd = 10;
    private const int NumberOfAdds = 10;

    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddDbContext<ApiDbContext>(
            options => options
                .UseSqlServer(
                    "Server=(localdb)\\mssqllocaldb;Database=graph-ql-database;Trusted_Connection=True;MultipleActiveResultSets=true")
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors());
        builder.Services.AddGraphQLServer()
            .AddQueryType<Query>()
            .AddFiltering()
            .AddSorting()
            .AddProjections();

        builder.Services.AddAuthorization();
        builder.Services.AddAutoMapper(typeof(Program));

        builder.Services.AddLogging(builder =>
        {
            builder.ClearProviders();
            builder.AddConsole();
            builder.AddDebug();
        });

        var app = builder.Build();

        var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
        var dbContext = scope.ServiceProvider.GetService<ApiDbContext>()!;
        dbContext.Database.Migrate();

        SeedData(dbContext);
        app.MapGraphQL();
        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.Run();
    }

    private static void SeedData(ApiDbContext dbContext)
    {
        var user = dbContext.Users.FirstOrDefault();
        if (user != null) return;

        for (var j = 0; j < NumberOfAdds; j++)
        {
            var toAdd = new List<User>();
            for (var i = 0; i < EntitiesToAdd; i++)
                toAdd.Add(
                    new User(Guid.NewGuid().ToString(), DateTimeOffset.UtcNow, $"{i}{j}",
                    [
                        new Post(Guid.NewGuid().ToString(), DateTimeOffset.UtcNow, $"Post {i}{j}"),
                        new Post(Guid.NewGuid().ToString(), DateTimeOffset.UtcNow, $"Post {i}{j}"),
                        new Post(Guid.NewGuid().ToString(), DateTimeOffset.UtcNow, $"Post {i}{j}")
                    ]));

            dbContext.Users.AddRange(toAdd);
            dbContext.SaveChanges();
        }
    }
}