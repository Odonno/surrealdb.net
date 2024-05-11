namespace SurrealDb.Net.Tests.Queryable;

public class FilteringQueryableTests : BaseQueryableTests
{
    [Fact]
    public void ShouldFilterWithStringConstantEquality()
    {
        string query = ToSurql(Posts.Where(p => p.Title == "Title 1"));

        // TODO : Remove the extra parenthesis
        query
            .Should()
            .Be(
                """
                SELECT * FROM post WHERE (Title == "Title 1")
                """
            );
    }

    [Fact]
    public void ShouldFilterWithMultipleBooleanLogic()
    {
        string query = ToSurql(Users.Where(u => (u.IsAdmin || u.IsOwner) && u.IsActive));

        // TODO : Remove the extra parenthesis
        query
            .Should()
            .Be(
                """
                SELECT * FROM user WHERE ((IsAdmin || IsOwner) && IsActive)
                """
            );
    }

    [Fact]
    public void ShouldFilterWithMultiplePredicates()
    {
        string query = ToSurql(
            Posts.Where(p => p.Title == "Title 1").Where(p => p.Status != "DRAFT")
        );

        // TODO : Remove the extra parenthesis
        query
            .Should()
            .Be(
                """
                SELECT * FROM post WHERE ((Title == "Title 1") && (Status != "DRAFT"))
                """
            );
    }
}
