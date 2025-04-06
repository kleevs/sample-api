namespace Company.SampleApi.Database.UserDb;

using System.Collections;
using System.Linq.Expressions;


public class AsyncEnumerator<TEntity> : IAsyncEnumerator<TEntity>
{
    private readonly IEnumerator<TEntity> _enumerator;

    public AsyncEnumerator(IEnumerator<TEntity> enumerator)
    {
        _enumerator = enumerator;
    }

    public TEntity Current => _enumerator.Current;

    public void Dispose()
    {
        _enumerator.Dispose();
    }

    public ValueTask DisposeAsync()
    {
        _enumerator.Dispose();
        return ValueTask.CompletedTask;
    }

    public ValueTask<bool> MoveNextAsync()
    {
        var result = _enumerator.MoveNext();
        return ValueTask.FromResult(result);
    }

    public Task<bool> MoveNextAsync(CancellationToken cancellationToken)
    {
        var result = _enumerator.MoveNext();
        return Task.FromResult(result);
    }
}

public class ProxyDbSet<TEntity> : IQueryable<TEntity>, IAsyncEnumerable<TEntity>
{
    private readonly IQueryable<TEntity> _query;

    public ProxyDbSet(IQueryable<TEntity> query)
    {
        _query = query;
    }

    Type IQueryable.ElementType => _query.ElementType;
    Expression IQueryable.Expression => _query.Expression;
    IQueryProvider IQueryable.Provider => _query.Provider;

    IAsyncEnumerator<TEntity> IAsyncEnumerable<TEntity>.GetAsyncEnumerator(CancellationToken cancellationToken) =>
        (_query as IAsyncEnumerable<TEntity>)?.GetAsyncEnumerator(cancellationToken) ?? new AsyncEnumerator<TEntity>(_query.GetEnumerator());

    IEnumerator<TEntity> IEnumerable<TEntity>.GetEnumerator() => (_query as IEnumerable<TEntity>)!.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => _query.GetEnumerator();
}

