namespace Api.GraphQL.Models;

public class UserModel
{
    public string Id { get; set; }
    public string Name { get; set; }
    public List<PostModel> Posts { get; set; }
}