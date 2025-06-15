using Moq;

namespace Company.SampleApi.Tools.Testing;

public static class MockExtension 
{
    public static void SetupQueryable<T, TQuery>(this Mock<TQuery> mock, IEnumerable<T> list) where TQuery : class, IQueryable<T>
    {
        var query = list.AsQueryable();
        mock.Setup(_ => _.ElementType).Returns(query.ElementType);
        mock.Setup(_ => _.Expression).Returns(query.Expression);
        mock.Setup(_ => _.Provider).Returns(query.Provider);
        mock.Setup(_ => _.GetEnumerator()).Returns(query.GetEnumerator());
    }
}
