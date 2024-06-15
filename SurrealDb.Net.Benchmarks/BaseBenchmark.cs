using System.Text;
using System.Text.Json.Serialization;
using Microsoft.Extensions.DependencyInjection;
using SurrealDb.Net.Benchmarks.Constants;

namespace SurrealDb.Net.Benchmarks;

public class BaseBenchmark
{
    public static string Host { get; } = "127.0.0.1:8000";
    protected string HttpUrl { get; } = $"http://{Host}";
    protected string WsUrl { get; } = $"ws://{Host}/rpc";
    protected string NamingPolicy { get; } = "SnakeCase";

    private readonly Func<JsonSerializerContext[]>? _funcJsonSerializerContexts;

    protected BaseBenchmark()
    {
        var isNativeAotRuntime = Environment.GetEnvironmentVariable(
            EnvVariablesConstants.NativeAotRuntime
        );

        _funcJsonSerializerContexts = string.IsNullOrWhiteSpace(isNativeAotRuntime)
            ? null
            : () => new JsonSerializerContext[] { AppJsonSerializerContext.Default };
    }

    protected Func<JsonSerializerContext[]>? GetFuncJsonSerializerContexts()
    {
        return _funcJsonSerializerContexts;
    }

    protected async Task CreatePostTable(string endpoint, DatabaseInfo databaseInfo)
    {
        var options = new SurrealDbOptionsBuilder()
            .WithEndpoint(endpoint)
            .WithNamespace(databaseInfo.Namespace)
            .WithDatabase(databaseInfo.Database)
            .WithUsername("root")
            .WithPassword("root")
            .WithNamingPolicy(NamingPolicy)
            .Build();

        var client = new SurrealDbClient(
            options,
            appendJsonSerializerContexts: GetFuncJsonSerializerContexts()
        );

        string query = GetPostQueryContent();
        await client.RawQuery(query);
    }

    protected async Task CreateEcommerceTables(string endpoint, DatabaseInfo databaseInfo)
    {
        var options = new SurrealDbOptionsBuilder()
            .WithEndpoint(endpoint)
            .WithNamespace(databaseInfo.Namespace)
            .WithDatabase(databaseInfo.Database)
            .WithUsername("root")
            .WithPassword("root")
            .WithNamingPolicy(NamingPolicy)
            .Build();

        var client = new SurrealDbClient(
            options,
            appendJsonSerializerContexts: GetFuncJsonSerializerContexts()
        );

        string query = GetEcommerceQueryContent();
        await client.RawQuery(query);
    }

    protected async Task<List<GeneratedPost>> SeedData(
        string endpoint,
        DatabaseInfo databaseInfo,
        int count = 1000
    )
    {
        var options = new SurrealDbOptionsBuilder()
            .WithEndpoint(endpoint)
            .WithNamespace(databaseInfo.Namespace)
            .WithDatabase(databaseInfo.Database)
            .WithUsername("root")
            .WithPassword("root")
            .WithNamingPolicy(NamingPolicy)
            .Build();

        var client = new SurrealDbClient(
            options,
            appendJsonSerializerContexts: GetFuncJsonSerializerContexts()
        );

        var tasks = new List<Task>();

        var generatedPosts = new PostFaker().Generate(count);

        generatedPosts.ForEach(
            (post) =>
            {
                string statement =
                    $"CREATE post SET title = \"{post.Title}\", content = \"{post.Content}\";";
                tasks.Add(client.RawQuery(statement));
            }
        );

        await Task.WhenAll(tasks);

        return generatedPosts;
    }

    protected async Task<Post> GetFirstPost(string httpUrl, DatabaseInfo databaseInfo)
    {
        var options = new SurrealDbOptionsBuilder()
            .WithEndpoint(httpUrl)
            .WithNamespace(databaseInfo.Namespace)
            .WithDatabase(databaseInfo.Database)
            .WithUsername("root")
            .WithPassword("root")
            .WithNamingPolicy(NamingPolicy)
            .Build();

        var client = new SurrealDbClient(
            options,
            appendJsonSerializerContexts: GetFuncJsonSerializerContexts()
        );

        var posts = await client.Select<Post>("post");
        return posts.First();
    }

    private static string GetPostQueryContent()
    {
        return GetFileContent("Schemas/post.surql");
    }

    private static string GetEcommerceQueryContent()
    {
        return GetFileContent("Schemas/ecommerce.surql");
    }

    private static string GetFileContent(string path)
    {
        string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path);
        return File.ReadAllText(filePath, Encoding.UTF8);
    }
}
