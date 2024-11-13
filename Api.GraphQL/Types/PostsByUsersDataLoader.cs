using Api.GraphQL.Data;
using Api.GraphQL.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace Api.GraphQL.Types;

public class PostsByUsersDataLoader : GroupedDataLoader<string, PostModel>
{
    private readonly ApiDbContext _dbContext;
    private readonly IMapper _mapper;

    public PostsByUsersDataLoader(
        IBatchScheduler batchScheduler,
        ApiDbContext dbContext,
        IMapper mapper,
        DataLoaderOptions? options = null) : base(batchScheduler, options)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    protected override async Task<ILookup<string, PostModel>> LoadGroupedBatchAsync(
        IReadOnlyList<string> keys,
        CancellationToken cancellationToken)
    {
        var posts = await _dbContext.Posts
            .Where(t => keys.Contains(t.UserId))
            .ProjectTo<PostModel>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return posts.ToLookup(p => p.UserId);
    }
}
