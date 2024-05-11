namespace SurrealDb.Net.Tests.Queryable;

public class PaginationQueryableTests : BaseQueryableTests
{
    [Fact]
    public void ShouldSkipFromTable()
    {
        string query = ToSurql(Posts.Skip(10));

        query
            .Should()
            .Be(
                """
                SELECT * FROM post START 10
                """
            );
    }

    [Fact]
    public void ShouldLimitFromTable()
    {
        string query = ToSurql(Posts.Take(5));

        query
            .Should()
            .Be(
                """
                SELECT * FROM post LIMIT 5
                """
            );
    }

    [Fact]
    public void ShouldLimitAndSkipFromTable()
    {
        string query = ToSurql(Posts.Skip(10).Take(5));

        query
            .Should()
            .Be(
                """
                SELECT * FROM post LIMIT 5 START 10
                """
            );
    }
}
