using Company.SampleApi.Contracts;
using Company.SampleApi.Entities;
using Company.SampleApi.Tools;

namespace Company.SampleApi;

public class SearchUsersHandler
{
    protected readonly IQueryable<User> _users;

    public SearchUsersHandler(IQueryable<User> users)
    {
        _users = users;
    }

    public async Task<IEnumerable<User>> HandleAsync(string? login) 
    {
        var query = _users;

        if (!string.IsNullOrEmpty(login))
        {
            query = query.Where(_ => _.Login.StartsWith(login));
        }

        return await query.ToListAsync();
    }
}
