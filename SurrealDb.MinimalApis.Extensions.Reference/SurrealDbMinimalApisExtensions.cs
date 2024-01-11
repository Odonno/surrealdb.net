﻿// <auto-generated/>

#nullable enable

using SurrealDb.MinimalApis.Extensions;
using SurrealDb.Net;
using SurrealDb.Net.Models;
using SystemTextJsonPatch;
using System.Text.Json;

namespace Microsoft.AspNetCore.Builder;

public static class SurrealDbMinimalApisExtensions
{
    /// <summary>
    /// Adds a complete list of <see cref="RouteEndpoint"/> to the <see cref="IEndpointRouteBuilder"/> that matches HTTP requests for the specified pattern.
    /// This method will add the following routes by default:
    /// <list type="table">
    /// <item>
    /// <term>GET /</term>
    /// <description>Select all records from a SurrealDB table.</description>
    /// </item>
    /// <item>
    /// <term>GET /{id}</term>
    /// <description>Select a single record from a SurrealDB table.</description>
    /// </item>
    /// <item>
    /// <term>POST /</term>
    /// <description>Create a new record in a SurrealDB table.</description>
    /// </item>
    /// <item>
    /// <term>PUT /</term>
    /// <description>Update a record in a SurrealDB table.</description>
    /// </item>
    /// <item>
    /// <term>PATCH /</term>
    /// <description>Patch all records in a SurrealDB table.</description>
    /// </item>
    /// <item>
    /// <term>PATCH /{id}</term>
    /// <description>Patch a single record in a SurrealDB table.</description>
    /// </item>
    /// <item>
    /// <term>DELETE /</term>
    /// <description>Delete all records from a SurrealDB table.</description>
    /// </item>
    /// <item>
    /// <term>DELETE /{id}</term>
    /// <description>Delete a single record from a SurrealDB table.</description>
    /// </item>
    /// </list>
    /// </summary>
    /// <typeparam name="TEntity">Type of the record stored in a SurrealDB instance.</typeparam>
    /// <param name="endpoints">The <see cref="IEndpointRouteBuilder"/> to add the route to.</param>
    /// <param name="pattern">The route pattern.</param>
    /// <param name="options">Options to customize the behavior of the different endpoints.</param>
    /// <returns>A <see cref="RouteHandlerBuilder"/> that can be used to further customize the endpoint.</returns>
    public static IEndpointRouteBuilder MapSurrealEndpoints<TEntity>(
        this IEndpointRouteBuilder endpoints,
        string pattern,
        SurrealDbMinimalApisOptions? options = null
    )
        where TEntity : Record
    {
        return endpoints.MapSurrealEndpoints<TEntity, ISurrealDbClient>(pattern, options);
    }

    /// <summary>
    /// Adds a complete list of <see cref="RouteEndpoint"/> to the <see cref="IEndpointRouteBuilder"/> that matches HTTP requests for the specified pattern.
    /// This method will add the following routes by default:
    /// <list type="table">
    /// <item>
    /// <term>GET /</term>
    /// <description>Select all records from a SurrealDB table.</description>
    /// </item>
    /// <item>
    /// <term>GET /{id}</term>
    /// <description>Select a single record from a SurrealDB table.</description>
    /// </item>
    /// <item>
    /// <term>POST /</term>
    /// <description>Create a new record in a SurrealDB table.</description>
    /// </item>
    /// <item>
    /// <term>PUT /</term>
    /// <description>Update a record in a SurrealDB table.</description>
    /// </item>
    /// <item>
    /// <term>PATCH /</term>
    /// <description>Patch all records in a SurrealDB table.</description>
    /// </item>
    /// <item>
    /// <term>PATCH /{id}</term>
    /// <description>Patch a single record in a SurrealDB table.</description>
    /// </item>
    /// <item>
    /// <term>DELETE /</term>
    /// <description>Delete all records from a SurrealDB table.</description>
    /// </item>
    /// <item>
    /// <term>DELETE /{id}</term>
    /// <description>Delete a single record from a SurrealDB table.</description>
    /// </item>
    /// </list>
    /// </summary>
    /// <typeparam name="TEntity">Type of the record stored in a SurrealDB instance.</typeparam>
    /// <typeparam name="TDbClient">Type of the SurrealDB client.</typeparam>
    /// <param name="endpoints">The <see cref="IEndpointRouteBuilder"/> to add the route to.</param>
    /// <param name="pattern">The route pattern.</param>
    /// <param name="options">Options to customize the behavior of the different endpoints.</param>
    /// <returns>A <see cref="RouteHandlerBuilder"/> that can be used to further customize the endpoint.</returns>
    public static IEndpointRouteBuilder MapSurrealEndpoints<TEntity, TDbClient>(
        this IEndpointRouteBuilder endpoints,
        string pattern,
        SurrealDbMinimalApisOptions? options = null
    )
        where TEntity : Record
        where TDbClient : ISurrealDbClient
    {
        string entityName = typeof(TEntity).Name;

        var group = endpoints.MapGroup(pattern);

        if (options?.Tags is not null)
        {
            group.WithTags(options.Tags);
        }

        if (options?.EnableGetAll ?? options?.EnableQueries ?? true)
        {
            group
                .MapGet(
                    "/",
                    (TDbClient surrealDbClient, CancellationToken cancellationToken) =>
                    {
                        string tableName = options?.TableName ?? GetDefaultTableName(entityName, surrealDbClient.NamingPolicy);
                        return surrealDbClient.Select<TEntity>(tableName).ToListAsync(cancellationToken);
                    }
                )
                .WithName($"GetAll{entityName}")
                .WithSummary($"Get all {entityName} records.")
                .Produces<IEnumerable<TEntity>>()
                .WithOpenApi();
        }

        if (options?.EnableGetSingle ?? options?.EnableQueries ?? true)
        {
            group
                .MapGet(
                    "/{id}",
                    async (
                        string id,
                        TDbClient surrealDbClient,
                        CancellationToken cancellationToken
                    ) =>
                    {
                        string tableName = options?.TableName ?? GetDefaultTableName(entityName, surrealDbClient.NamingPolicy);
                        var data = await surrealDbClient.Select<TEntity>(
                            (tableName, id),
                            cancellationToken
                        );

                        if (data is null)
                            return Results.NotFound();

                        return Results.Ok(data);
                    }
                )
                .WithName($"Get{entityName}")
                .WithSummary($"Get a single {entityName} record.")
                .Produces<TEntity>()
                .ProducesProblem(StatusCodes.Status404NotFound)
                .WithOpenApi();
        }

        if (options?.EnablePost ?? options?.EnableMutations ?? true)
        {
            group
                .MapPost(
                    "/",
                    (
                        TEntity data,
                        TDbClient surrealDbClient,
                        CancellationToken cancellationToken
                    ) =>
                    {
                        string tableName = options?.TableName ?? GetDefaultTableName(entityName, surrealDbClient.NamingPolicy);
                        return surrealDbClient.Create(tableName, data, cancellationToken);
                    }
                )
                .WithName($"Create{entityName}")
                .WithSummary($"Create a {entityName} record.")
                .Produces<TEntity>()
                .WithOpenApi();
        }

        if (options?.EnablePut ?? options?.EnableMutations ?? true)
        {
            group
                .MapPut(
                    "/",
                    (
                        TEntity data,
                        TDbClient surrealDbClient,
                        CancellationToken cancellationToken
                    ) =>
                    {
                        return surrealDbClient.Upsert(data, cancellationToken);
                    }
                )
                .WithName($"Update{entityName}")
                .WithSummary($"Update a {entityName} record.")
                .Produces<TEntity>()
                .WithOpenApi();
        }

        if (options?.EnablePatchAll ?? options?.EnableMutations ?? true)
        {
            group
                .MapPatch(
                    "/",
                    (
                        JsonPatchDocument<TEntity> patches,
                        TDbClient surrealDbClient,
                        CancellationToken cancellationToken
                    ) =>
                    {
                        string tableName = options?.TableName ?? GetDefaultTableName(entityName, surrealDbClient.NamingPolicy);
                        return surrealDbClient.PatchAll(tableName, patches, cancellationToken);
                    }
                )
                .WithName($"PatchAll{entityName}")
                .WithSummary($"Patch all {entityName} records.")
                .Produces<IEnumerable<TEntity>>()
                .WithOpenApi();
        }

        if (options?.EnablePatchSingle ?? options?.EnableMutations ?? true)
        {
            group
                .MapPatch(
                    "/{id}",
                    (
                        string id,
                        JsonPatchDocument<TEntity> patches,
                        TDbClient surrealDbClient,
                        CancellationToken cancellationToken
                    ) =>
                    {
                        string tableName = options?.TableName ?? GetDefaultTableName(entityName, surrealDbClient.NamingPolicy);
                        return surrealDbClient.Patch((tableName, id), patches, cancellationToken);
                    }
                )
                .WithName($"Patch{entityName}")
                .WithSummary($"Patch a single {entityName} record.")
                .Produces<TEntity>()
                .WithOpenApi();
        }

        if (options?.EnableDeleteAll ?? options?.EnableMutations ?? true)
        {
            group
                .MapDelete(
                    "/",
                    (TDbClient surrealDbClient, CancellationToken cancellationToken) =>
                    {
                        string tableName = options?.TableName ?? GetDefaultTableName(entityName, surrealDbClient.NamingPolicy);
                        return surrealDbClient.Delete(tableName, cancellationToken);
                    }
                )
                .WithName($"DeleteAll{entityName}")
                .WithSummary($"Delete all {entityName} records.")
                .Produces(StatusCodes.Status200OK)
                .WithOpenApi();
        }

        if (options?.EnableDeleteSingle ?? options?.EnableMutations ?? true)
        {
            group
                .MapDelete(
                    "/{id}",
                    async (
                        string id,
                        TDbClient surrealDbClient,
                        CancellationToken cancellationToken
                    ) =>
                    {
                        string tableName = options?.TableName ?? GetDefaultTableName(entityName, surrealDbClient.NamingPolicy);
                        bool success = await surrealDbClient.Delete(
                            (tableName, id),
                            cancellationToken
                        );

                        if (!success)
                            return Results.NotFound();

                        return Results.Ok();
                    }
                )
                .WithName($"Delete{entityName}")
                .WithSummary($"Delete a {entityName} record.")
                .Produces(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status404NotFound)
                .WithOpenApi();
        }

        return endpoints;
    }

    private static string GetDefaultTableName(string entityName, string? namingPolicy)
    {
        return (namingPolicy?.ToLowerInvariant()) switch
        {
            "camelcase" => entityName.ToCamelCase(),
            "snakecase" or "snakecaselower" => entityName.ToSnakeCaseLower(),
            "snakecaseupper" => entityName.ToSnakeCaseUpper(),
            "kebabcase" or "kebabcaselower" => entityName.ToKebabCaseLower(),
            "kebabcaseupper" => entityName.ToKebabCaseUpper(),
            _ => entityName
        };
    }

    private static string ToCamelCase(this string str)
    {
        return JsonNamingPolicy.CamelCase.ConvertName(str);
    }

    private static string ToSnakeCaseLower(this string str)
    {
        return JsonNamingPolicy.SnakeCaseLower.ConvertName(str);
    }

    private static string ToSnakeCaseUpper(this string str)
    {
        return JsonNamingPolicy.SnakeCaseUpper.ConvertName(str);
    }

    private static string ToKebabCaseLower(this string str)
    {
        return JsonNamingPolicy.KebabCaseLower.ConvertName(str);
    }

    private static string ToKebabCaseUpper(this string str)
    {
        return JsonNamingPolicy.KebabCaseUpper.ConvertName(str);
    }
}
