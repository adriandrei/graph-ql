using System.ComponentModel.DataAnnotations;

namespace Api.GraphQL.Data.Entities;

public abstract class BaseEntity
{
    public BaseEntity()
    {
    }

    public BaseEntity(string id, DateTimeOffset updatedAt)
    {
        Id = id;
        UpdatedAt = updatedAt;
    }

    [Key] public string Id { get; set; }

    public DateTimeOffset UpdatedAt { get; set; }
}