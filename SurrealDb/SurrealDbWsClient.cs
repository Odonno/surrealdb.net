using SurrealDb.Internals.Helpers;

namespace SurrealDb;

public static class SurrealDbWsClient
{
	/// <summary>
	/// Creates a new SurrealDbClient using the WS protocol.
	/// </summary>
	/// <param name="host">The host name of the SurrealDB instance.</param>
	/// <param name="ns">The table namespace to connect to.</param>
	/// <param name="db">The table database to connect to.</param>
	/// <param name="username">The username to connect to (with root access).</param>
	/// <param name="password">The password to connect to (with root access).</param>
	/// <exception cref="ArgumentException"></exception>
	public static ISurrealDbClient New(string host, string? ns = null, string? db = null, string? username = null, string? password = null)
	{
#if NET6_0_OR_GREATER
		string endpoint = UriBuilderHelper.CreateEndpointFromProtocolAndHost(host, Uri.UriSchemeWs, "/rpc");
#else
		const string protocol = "ws";
		string endpoint = UriBuilderHelper.CreateEndpointFromProtocolAndHost(host, protocol, "/rpc");
#endif

		return new SurrealDbClient(endpoint, ns, db, username, password, null);
	}
}
