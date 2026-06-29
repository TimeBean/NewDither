using Service.Quote.Domain.Model.Numeric;

namespace Service.Quote.Domain.Model;

/// <summary>
/// Represents a quote with its author, text content, and the year it was created.
/// </summary>
public class Quote
{
    /// <summary>
    /// Gets the author who said or wrote the quote.
    /// </summary>
    public Author Author { get; }

    /// <summary>
    /// Gets the actual text content of the quote.
    /// </summary>
    public string Content { get; }

    /// <summary>
    /// Gets the year the quote originates from.
    /// </summary>
    public Year Year { get; }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="Quote"/> class.
    /// </summary>    
    /// <param name="author">The author who said or wrote the quote.</param>
    /// <param name="content">The actual text content of the quote.</param>
    /// <param name="year">The year the quote originates from.</param>
    public Quote(Author author, string content, Year year)
    {
        Author = author;
        Content = content;
        Year = year;
    }

    override public string ToString()
    {
        return $"\"{Content}\" — {Author}, {Year}";
    }
}