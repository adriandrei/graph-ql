using Api.GraphQL.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Api.GraphQL.Data;

public class ApiDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Post> Posts { get; set; }

    public ApiDbContext(DbContextOptions options) : base(options)
    {
    }
}
