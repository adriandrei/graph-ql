namespace Api.GraphQL.Data.Entities;

public class User : BaseEntity
{
    public User()
    {
    }

    public User(
        string id,
        DateTimeOffset updatedAt,
        string name,
        List<Post> posts) : base(id, updatedAt)
    {
        Name = name;
        Posts = posts;
    }

    public string Name { get; set; }
    public List<Post> Posts { get; set; }
}