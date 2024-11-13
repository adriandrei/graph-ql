using Api.GraphQL.Models;

namespace Api.GraphQL.Types;

[ExtendObjectType(typeof(UserModel))]
public class UserTypeExtensions
{
    public Task<PostModel[]?> GetPostsWithDataLoader(
        [Parent] UserModel user,
        [Service] PostsByUsersDataLoader dataLoader,
        CancellationToken cancellationToken)
    => dataLoader.LoadAsync(user.Id, cancellationToken);
}
