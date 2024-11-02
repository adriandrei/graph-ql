using System.Reflection;
using Api.GraphQL.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Api.GraphQL.Data;

public class ApiDbContext : DbContext
{
    public ApiDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Post> Posts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}