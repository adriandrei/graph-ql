using System.ComponentModel.DataAnnotations;

namespace Api.GraphQL.Data.Entities;

public class BaseEntity
{
    public BaseEntity()
    {
    }
    public BaseEntity(string id, DateTimeOffset updatedAt)
    {
        Id = id;
        UpdatedAt = updatedAt;
    }

    [Key]
    public string Id { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}

public class User : BaseEntity
{
    public User()
    {
        
    }
    public User(
        string id, 
        DateTimeOffset updatedAt,
        string name) : base(id, updatedAt)
    {
        Name = name;
    }

    public string Name { get; set; }
}

public class Post : BaseEntity
{
    public Post()
    {
        
    }
    public Post(
        string id, 
        DateTimeOffset updatedAt,
        string text) : base(id, updatedAt)
    {
        Text = text;
    }

    public string Text { get; set; }
}
