namespace SurrealDb.Net.Tests.Queryable;

public class GroupingQueryableTests : BaseQueryableTests
{
    [Test]
    [Skip("TODO")]
    public void DefaultGroupingBySingleField()
    {
        string query = ToSurql(Posts.GroupBy(p => p.Status));

        const string fieldsProjection =
            "content AS Content, created_at AS CreatedAt, id AS Id, status AS Status, title AS Title";
        const string fullProjection = $"SELECT {fieldsProjection} FROM post";

        query
            .Should()
            .Be(
                $"""
                SELECT status AS Key, ({fullProjection} WHERE status == $parent.status) AS Values FROM (SELECT status FROM post GROUP BY status)
                """
            );
    }

    [Test]
    [Skip("TODO")]
    public void ShouldGroupBySingleField()
    {
        string query = ToSurql(Posts.GroupBy(p => p.Status).Select(g => g.Key));

        query
            .Should()
            .Be(
                """
                SELECT VALUE Status FROM (SELECT Status FROM post GROUP BY Status)
                """
            );
    }

    [Test]
    [Skip("TODO")]
    public void ShouldGroupByMultipleFields()
    {
        string query = ToSurql(
            Addresses.GroupBy(a => new { a.Country, a.City }).Select(g => g.Key)
        );

        query
            .Should()
            .Be(
                """
                SELECT Country, City FROM address GROUP BY Country, City
                """
            );
    }

    [Test]
    [Skip("TODO")]
    public void ShouldGroupBySingleFieldAndAggregateCountViaProjection()
    {
        string query = ToSurql(Posts.GroupBy(p => p.Status).Select(g => g.Count()));

        query
            .Should()
            .Be(
                """
                SELECT count() FROM post GROUP BY Status
                """
            );
    }

    // TODO : Sum, Min, Max, Avg

    // TODO : Project to anonymous type

    // [Test]
    // [Skip("TODO")]
    // public void ShouldGroupBySingleFieldAndAggregateSumViaProjection()
    // {
    //     string query = ToSurql(Posts.GroupBy(p => p.Status).Select(g => g..Sum()));
    //
    //     query
    //         .Should()
    //         .Be(
    //             """
    //             SELECT count() FROM post GROUP BY Status
    //             """
    //         );
    // }
}
