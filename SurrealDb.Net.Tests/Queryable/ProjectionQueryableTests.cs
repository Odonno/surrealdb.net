namespace SurrealDb.Net.Tests.Queryable;

public class ProjectionQueryableTests : BaseQueryableTests
{
    [Fact]
    public void ShouldAllFieldsFromTable()
    {
        string query = ToSurql(Users);

        query
            .Should()
            .Be(
                """
                SELECT * FROM user
                """
            );
    }

    [Fact(Skip = "TODO")]
    public void ShouldSelectFieldsUsingLambdaExpression()
    {
        string query = ToSurql(Users.Select(u => new { u.Username, u.Age }));

        query
            .Should()
            .Be(
                """
                SELECT Username, Age FROM user
                """
            );
    }
}
