using Microsoft.EntityFrameworkCore;

namespace Company.SampleApi.Tools;

public static class QueryableExtension
{
    public static Task<List<T>> ToListAsync<T>(this IQueryable<T> query, CancellationToken cancellationToken = default)
    {
        if (query is IAsyncEnumerable<T>)
        {
            return EntityFrameworkQueryableExtensions.ToListAsync(query, cancellationToken);
        }

        return Task.Run(() => query.ToList());
    }

    public static Task<T?> FirstOrDefaultAsync<T>(this IQueryable<T> query, CancellationToken cancellationToken = default)
    {
        if (query is IAsyncEnumerable<T>)
        {
            return EntityFrameworkQueryableExtensions.FirstOrDefaultAsync(query, cancellationToken);
        }

        return Task.Run(() => query.FirstOrDefault());
    }
}
