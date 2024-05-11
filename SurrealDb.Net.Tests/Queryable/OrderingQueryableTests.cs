namespace SurrealDb.Net.Tests.Queryable;

public class OrderingQueryableTests : BaseQueryableTests
{
    [Fact]
    public void ShouldOrderByAsc()
    {
        string query = ToSurql(Posts.OrderBy(p => p.CreatedAt));

        query
            .Should()
            .Be(
                """
                SELECT * FROM post ORDER BY CreatedAt
                """
            );
    }

    [Fact]
    public void ShouldOrderByDesc()
    {
        string query = ToSurql(Posts.OrderByDescending(p => p.CreatedAt));

        query
            .Should()
            .Be(
                """
                SELECT * FROM post ORDER BY CreatedAt DESC
                """
            );
    }

    [Fact]
    public void ShouldOrderByMultipleFieldsAsc()
    {
        string query = ToSurql(
            Posts.OrderBy(p => p.CreatedAt).ThenBy(p => p.Status).ThenBy(p => p.Title)
        );

        query
            .Should()
            .Be(
                """
                SELECT * FROM post ORDER BY CreatedAt, Status, Title
                """
            );
    }

    [Fact]
    public void ShouldOrderByMultipleFieldsEitherAscOrDesc()
    {
        string query = ToSurql(
            Posts
                .OrderByDescending(p => p.CreatedAt)
                .ThenByDescending(p => p.Status)
                .ThenBy(p => p.Title)
        );

        query
            .Should()
            .Be(
                """
                SELECT * FROM post ORDER BY CreatedAt DESC, Status DESC, Title
                """
            );
    }
}
