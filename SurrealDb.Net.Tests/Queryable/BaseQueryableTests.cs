using SurrealDb.Net.Internals.Query;

namespace SurrealDb.Net.Tests.Queryable;

public class User : SurrealDbRecord
{
    public string Username { get; set; } = string.Empty;
    public bool IsAdmin { get; set; }
    public bool IsActive { get; set; }
    public bool IsOwner { get; set; }
    public int Age { get; set; }
}

public class Address : SurrealDbRecord
{
    public int Number { get; set; }
    public string Street { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string ZipCode { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}

public abstract class BaseQueryableTests
{
    private const string PostTableName = "post";
    private const string UserTableName = "user";
    private const string AddressTableName = "address";

    private readonly Lazy<IQueryable<Post>> _lazyPosts = new(CreateQueryable<Post>(PostTableName));
    private readonly Lazy<IQueryable<User>> _lazyUsers = new(CreateQueryable<User>(UserTableName));
    private readonly Lazy<IQueryable<Address>> _lazyAddresses =
        new(CreateQueryable<Address>(AddressTableName));

    public IQueryable<Post> Posts => _lazyPosts.Value;
    public IQueryable<User> Users => _lazyUsers.Value;
    public IQueryable<Address> Addresses => _lazyAddresses.Value;

    public string ToSurql<T>(IQueryable<T> queryable)
    {
        var tableName = queryable.Provider switch
        {
            IQueryProvider qp when qp == Posts.Provider => PostTableName,
            IQueryProvider qp when qp == Users.Provider => UserTableName,
            IQueryProvider qp when qp == Addresses.Provider => AddressTableName,
            _ => null
        };

        if (string.IsNullOrWhiteSpace(tableName))
        {
            throw new InvalidOperationException("Invalid table name");
        }

        return new QueryGeneratorExpressionVisitor().Visit(queryable.Expression, tableName);
    }

    private static SurrealDbQueryable<T> CreateQueryable<T>(string table)
    {
        return new(new SurrealDbQueryProvider<T>(null!, table));
    }
}
