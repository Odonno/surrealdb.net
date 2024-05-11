namespace SurrealDb.Net.Tests.Queryable;

// 💡 Here is a list of non-supported operators:
// * "?:" - conditional ternary operator (https://docs.surrealdb.com/docs/surrealql/operators#tco)
// * "=" - simple equality operator without type matching (https://docs.surrealdb.com/docs/surrealql/operators#equal)
// * "?=" - value existence in array (https://docs.surrealdb.com/docs/surrealql/operators#anyequal)
// * "*=" - all values are the same in array (https://docs.surrealdb.com/docs/surrealql/operators#allequal)
// * "~" - fuzzy matching (https://docs.surrealdb.com/docs/surrealql/operators#match)
// * "!~" - inequality fuzzy matching (https://docs.surrealdb.com/docs/surrealql/operators#notmatch)
// * "?~" - value existence in array using fuzzy matching (https://docs.surrealdb.com/docs/surrealql/operators#notmatch)
// * "*~" - all values are the same in array using fuzzy matching (https://docs.surrealdb.com/docs/surrealql/operators#allmatch)

public class OperatorQueryableTests : BaseQueryableTests
{
    [Fact]
    public void ShouldHandleAndOperator()
    {
        string query = ToSurql(Users.Where(u => u.IsAdmin && u.IsActive));

        // TODO : Remove the extra parenthesis
        query
            .Should()
            .Be(
                """
                SELECT * FROM user WHERE (IsAdmin && IsActive)
                """
            );
    }

    [Fact]
    public void ShouldHandleOrOperator()
    {
        string query = ToSurql(Users.Where(u => u.IsAdmin || u.IsOwner));

        // TODO : Remove the extra parenthesis
        query
            .Should()
            .Be(
                """
                SELECT * FROM user WHERE (IsAdmin || IsOwner)
                """
            );
    }

    [Fact(Skip = "TODO")]
    public void ShouldHandleNullCoalescingOperator()
    {
        string query = ToSurql(Posts.Select(p => p.Status ?? "DRAFT"));

        query
            .Should()
            .Be(
                """
                SELECT status ?? DRAFT FROM post
                """
            );
    }

    [Fact]
    public void ShouldHandleUnequalityOperator()
    {
        string query = ToSurql(Posts.Where(p => p.Title != "Title 1"));

        // TODO : Remove the extra parenthesis
        query
            .Should()
            .Be(
                """
                SELECT * FROM post WHERE (Title != "Title 1")
                """
            );
    }

    [Fact]
    public void ShouldHandleEqualityOperator()
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
    public void ShouldHandleLessThanOperator()
    {
        string query = ToSurql(Addresses.Where(a => a.Number < 15));

        // TODO : Remove the extra parenthesis
        query
            .Should()
            .Be(
                """
                SELECT * FROM address WHERE (Number < 15)
                """
            );
    }

    [Fact]
    public void ShouldHandleLessThanOrEqualOperator()
    {
        string query = ToSurql(Addresses.Where(a => a.Number <= 15));

        // TODO : Remove the extra parenthesis
        query
            .Should()
            .Be(
                """
                SELECT * FROM address WHERE (Number <= 15)
                """
            );
    }

    [Fact]
    public void ShouldHandleGreaterThanOperator()
    {
        string query = ToSurql(Addresses.Where(a => a.Number > 15));

        // TODO : Remove the extra parenthesis
        query
            .Should()
            .Be(
                """
                SELECT * FROM address WHERE (Number > 15)
                """
            );
    }

    [Fact]
    public void ShouldHandleGreaterThanOrEqualOperator()
    {
        string query = ToSurql(Addresses.Where(a => a.Number >= 15));

        // TODO : Remove the extra parenthesis
        query
            .Should()
            .Be(
                """
                SELECT * FROM address WHERE (Number >= 15)
                """
            );
    }

    [Fact(Skip = "TODO")]
    public void ShouldHandleAddOperator()
    {
        string query = ToSurql(Addresses.Select(a => a.State + " " + a.ZipCode));

        // TODO : Remove the extra parenthesis
        query
            .Should()
            .Be(
                """
                SELECT VALUE ((State + " ") + ZipCode) FROM address
                """
            );
    }

    [Fact(Skip = "TODO")]
    public void ShouldHandleSubstractOperator()
    {
        string query = ToSurql(Posts.Select(p => p.CreatedAt - TimeSpan.FromDays(30)));

        // TODO : Remove the extra parenthesis
        query
            .Should()
            .Be(
                """
                SELECT VALUE (CreatedAt - 4w2d) FROM post
                """
            );
    }

    [Fact(Skip = "TODO")]
    public void ShouldHandleMultiplyOperator()
    {
        string query = ToSurql(Users.Select(u => u.Age * 2));

        // TODO : Remove the extra parenthesis
        query
            .Should()
            .Be(
                """
                SELECT VALUE (Age * 2) FROM user
                """
            );
    }

    [Fact(Skip = "TODO")]
    public void ShouldHandleDivideOperator()
    {
        string query = ToSurql(Users.Select(u => u.Age / 2));

        // TODO : Remove the extra parenthesis
        query
            .Should()
            .Be(
                """
                SELECT VALUE (Age / 2) FROM user
                """
            );
    }

    [Fact(Skip = "TODO")]
    public void ShouldHandleModuloOperator()
    {
        string query = ToSurql(Users.Select(u => u.Age % 2));

        // TODO : Remove the extra parenthesis
        query
            .Should()
            .Be(
                """
                SELECT VALUE (Age % 2) FROM user
                """
            );
    }
}
