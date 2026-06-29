namespace Service.Quote.Domain.Exception;

/// <summary>
/// The exception that is thrown when a requested quote cannot be found.
/// </summary>
public class QuoteNotFoundException : System.Exception
{
    /// <summary>
    /// Gets the identifier of the quote that was not found.
    /// </summary>
    public int? QuoteId { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="QuoteNotFoundException"/> class.
    /// </summary>
    public QuoteNotFoundException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="QuoteNotFoundException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public QuoteNotFoundException(string? message) : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="QuoteNotFoundException"/> class with the identifier of the missing quote.
    /// </summary>
    /// <param name="id">The unique identifier of the quote that was not found.</param>
    public QuoteNotFoundException(int id) : base($"Quote with ID {id} was not found.")
    {
        QuoteId = id;
    }
}