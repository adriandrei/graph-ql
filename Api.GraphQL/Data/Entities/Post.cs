namespace Api.GraphQL.Data.Entities;

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