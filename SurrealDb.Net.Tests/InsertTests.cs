﻿using System.Text;

namespace SurrealDb.Net.Tests;

public class InsertTests
{
    [Test]
    [ConnectionStringFixtureGenerator]
    public async Task ShouldInsertZeroRecord(string connectionString)
    {
        IEnumerable<Empty>? list = null;
        IEnumerable<Empty>? result = null;

        Func<Task> func = async () =>
        {
            await using var surrealDbClientGenerator = new SurrealDbClientGenerator();
            var dbInfo = surrealDbClientGenerator.GenerateDatabaseInfo();

            using var client = surrealDbClientGenerator.Create(connectionString);
            await client.Use(dbInfo.Namespace, dbInfo.Database);

            await client.ApplySchemaAsync(SurrealSchemaFile.Post);

            result = await client.Insert("empty", Enumerable.Empty<Empty>());

            list = await client.Select<Empty>("empty");
        };

        await func.Should().NotThrowAsync();

        list.Should().NotBeNull().And.HaveCount(0);

        result.Should().NotBeNull().And.HaveCount(0);
    }

    [Test]
    [ConnectionStringFixtureGenerator]
    public async Task ShouldInsertPostsWithAutogeneratedId(string connectionString)
    {
        IEnumerable<Post>? list = null;
        IEnumerable<Post>? result = null;

        Func<Task> func = async () =>
        {
            await using var surrealDbClientGenerator = new SurrealDbClientGenerator();
            var dbInfo = surrealDbClientGenerator.GenerateDatabaseInfo();

            using var client = surrealDbClientGenerator.Create(connectionString);
            await client.Use(dbInfo.Namespace, dbInfo.Database);

            await client.ApplySchemaAsync(SurrealSchemaFile.Post);

            var post = new Post
            {
                Title = "A new article",
                Content = "This is a new article created using the .NET SDK",
            };

            result = await client.Insert("post", new[] { post });

            list = await client.Select<Post>("post");
        };

        await func.Should().NotThrowAsync();

        list.Should().NotBeNull().And.HaveCount(3);

        result.Should().NotBeNull().And.HaveCount(1);

        var post = result!.First();
        post!.Title.Should().Be("A new article");
        post!.Content.Should().Be("This is a new article created using the .NET SDK");
        post!.CreatedAt.Should().NotBeNull();
        post!.Status.Should().Be("DRAFT");
    }

    [Test]
    [ConnectionStringFixtureGenerator]
    public async Task ShouldInsertPostsWithPredefinedId(string connectionString)
    {
        IEnumerable<Post>? list = null;
        IEnumerable<Post>? result = null;

        Func<Task> func = async () =>
        {
            await using var surrealDbClientGenerator = new SurrealDbClientGenerator();
            var dbInfo = surrealDbClientGenerator.GenerateDatabaseInfo();

            using var client = surrealDbClientGenerator.Create(connectionString);
            await client.Use(dbInfo.Namespace, dbInfo.Database);

            await client.ApplySchemaAsync(SurrealSchemaFile.Post);

            var post = new Post
            {
                Id = ("post", "another"),
                Title = "A new article",
                Content = "This is a new article created using the .NET SDK",
            };

            result = await client.Insert("post", new[] { post });

            list = await client.Select<Post>("post");
        };

        await func.Should().NotThrowAsync();

        list.Should().NotBeNull().And.HaveCount(3);

        result.Should().NotBeNull().And.HaveCount(1);

        var post = result!.First();
        post!.Title.Should().Be("A new article");
        post!.Content.Should().Be("This is a new article created using the .NET SDK");
        post!.CreatedAt.Should().NotBeNull();
        post!.Status.Should().Be("DRAFT");

        var anotherPost = list!.First(r => r.Id!.DeserializeId<string>() == "another");

        anotherPost.Should().NotBeNull();
        anotherPost!.Title.Should().Be("A new article");
        anotherPost!.Content.Should().Be("This is a new article created using the .NET SDK");
        anotherPost!.CreatedAt.Should().NotBeNull();
        anotherPost!.Status.Should().Be("DRAFT");
    }

    [Test]
    [ConnectionStringFixtureGenerator]
    public async Task ShouldInsertMultiplePosts(string connectionString)
    {
        IEnumerable<Post>? result = null;
        IEnumerable<Post>? list = null;

        Func<Task> func = async () =>
        {
            await using var surrealDbClientGenerator = new SurrealDbClientGenerator();
            var dbInfo = surrealDbClientGenerator.GenerateDatabaseInfo();

            using var client = surrealDbClientGenerator.Create(connectionString);
            await client.Use(dbInfo.Namespace, dbInfo.Database);

            await client.ApplySchemaAsync(SurrealSchemaFile.Post);

            var posts = new List<Post>
            {
                new Post
                {
                    Id = ("post", "A"),
                    Title = "An article",
                    Content = "This is a new article",
                },
                new Post
                {
                    Id = ("post", "B"),
                    Title = "An article",
                    Content = "This is a new article",
                },
                new Post
                {
                    Id = ("post", "C"),
                    Title = "An article",
                    Content = "This is a new article",
                },
            };

            result = await client.Insert("post", posts);

            list = await client.Select<Post>("post");
        };

        await func.Should().NotThrowAsync();

        result.Should().NotBeNull().And.HaveCount(3);
        list.Should().NotBeNull().And.HaveCount(5);
    }
}
