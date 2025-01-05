using Api.GraphQL.Data;
using Api.GraphQL.Data.Entities;
using Api.GraphQL.Types;
using Microsoft.EntityFrameworkCore;

namespace Api.GraphQL;

public class Program
{
    private const int EntitiesToAdd = 0;
    private const int NumberOfAdds = 0;

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
            .AddType<QueryTypeExtensions>()
            .AddDataLoader<PostsByUsersDataLoader>()
            .AddTypeExtension<UserTypeExtensions>()
            .AddFiltering()
            .AddSorting()
            .AddProjections();

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", policy =>
            {
                policy.AllowAnyOrigin() // Allow requests from any origin
                      .AllowAnyHeader() // Allow any headers
                      .AllowAnyMethod(); // Allow any HTTP methods (GET, POST, etc.)
            });
        });
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

        Task.Run(() => SeedData(dbContext));
        app.UseCors("AllowAll"); // Apply the CORS policy
        app.MapGraphQL();
        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.Run();
    }

    private static void SeedData(ApiDbContext dbContext)
    {
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