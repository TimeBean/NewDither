namespace Service.Quote.Domain.Repository;

/// <summary>
/// Defines the repository for managing quotes.
/// </summary>
public interface IQuoteRepository
{
    /// <summary>
    /// Retrieves a quote by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the quote.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the <see cref="Model.Quote"/>.</returns>
    Task<Model.Quote> Get(int id);

    /// <summary>
    /// Gets the total number of quotes in the repository.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains the total count of quotes.</returns>
    Task<int> Count();

    /// <summary>
    /// Retrieves all quotes belonging to a specific author.
    /// </summary>
    /// <param name="authorId">The unique identifier of the author.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a collection of the author's quotes.</returns>
    Task<IEnumerable<Model.Quote>> GetAllOfAuthor(int authorId);
}