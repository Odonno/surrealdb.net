namespace SurrealDb.Net;

// 💡 Inspired by https://github.com/dotnet/efcore/blob/f96570aecfc93fe49fbaa5f1f9515b3a3f3c038e/src/EFCore/Extensions/EntityFrameworkQueryableExtensions.cs

public static class QueryableExtensions
{
    #region AsAsyncEnumerable

    /// <summary>
    ///     Returns an <see cref="IAsyncEnumerable{T}" /> which can be enumerated asynchronously.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Multiple active operations on the same context instance are not supported. Use <see langword="await" /> to ensure
    ///         that any asynchronous operations have completed before calling another method on this context.
    ///         See <see href="https://aka.ms/efcore-docs-threading">Avoiding DbContext threading issues</see> for more information and examples.
    ///     </para>
    ///     <para>
    ///         See <see href="https://aka.ms/efcore-docs-async-linq">Querying data with EF Core</see> for more information and examples.
    ///     </para>
    /// </remarks>
    /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
    /// <param name="source">An <see cref="IQueryable{T}" /> to enumerate.</param>
    /// <returns>The query results.</returns>
    /// <exception cref="InvalidOperationException"><paramref name="source" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="source" /> is not a <see cref="IAsyncEnumerable{T}" />.</exception>
    public static IAsyncEnumerable<TSource> AsAsyncEnumerable<TSource>(
        this IQueryable<TSource> source
    )
    {
        if (source is IAsyncEnumerable<TSource> asyncEnumerable)
        {
            return asyncEnumerable;
        }

        throw new InvalidOperationException("This IQueryable does not handle async");
    }

    #endregion

    #region ToList/Array

    /// <summary>
    ///     Asynchronously creates a <see cref="List{T}" /> from an <see cref="IQueryable{T}" /> by enumerating it
    ///     asynchronously.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Multiple active operations on the same context instance are not supported. Use <see langword="await" /> to ensure
    ///         that any asynchronous operations have completed before calling another method on this context.
    ///         See <see href="https://aka.ms/efcore-docs-threading">Avoiding DbContext threading issues</see> for more information and examples.
    ///     </para>
    ///     <para>
    ///         See <see href="https://aka.ms/efcore-docs-async-linq">Querying data with EF Core</see> for more information and examples.
    ///     </para>
    /// </remarks>
    /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
    /// <param name="source">An <see cref="IQueryable{T}" /> to create a list from.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation.
    ///     The task result contains a <see cref="List{T}" /> that contains elements from the input sequence.
    /// </returns>
    /// <exception cref="ArgumentNullException"><paramref name="source" /> is <see langword="null" />.</exception>
    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
    public static async Task<List<TSource>> ToListAsync<TSource>(
        this IQueryable<TSource> source,
        CancellationToken cancellationToken = default
    )
    {
        var list = new List<TSource>();
        await foreach (
            var element in source
                .AsAsyncEnumerable()
                .WithCancellation(cancellationToken)
                .ConfigureAwait(false)
        )
        {
            list.Add(element);
        }

        return list;
    }

    /// <summary>
    ///     Asynchronously creates an array from an <see cref="IQueryable{T}" /> by enumerating it asynchronously.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Multiple active operations on the same context instance are not supported. Use <see langword="await" /> to ensure
    ///         that any asynchronous operations have completed before calling another method on this context.
    ///         See <see href="https://aka.ms/efcore-docs-threading">Avoiding DbContext threading issues</see> for more information and examples.
    ///     </para>
    ///     <para>
    ///         See <see href="https://aka.ms/efcore-docs-async-linq">Querying data with EF Core</see> for more information and examples.
    ///     </para>
    /// </remarks>
    /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
    /// <param name="source">An <see cref="IQueryable{T}" /> to create an array from.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation.
    ///     The task result contains an array that contains elements from the input sequence.
    /// </returns>
    /// <exception cref="ArgumentNullException"><paramref name="source" /> is <see langword="null" />.</exception>
    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
    public static async Task<TSource[]> ToArrayAsync<TSource>(
        this IQueryable<TSource> source,
        CancellationToken cancellationToken = default
    ) => (await source.ToListAsync(cancellationToken).ConfigureAwait(false)).ToArray();

    #endregion
}
