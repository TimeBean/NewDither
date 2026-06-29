namespace Service.Quote.Domain.Model;

/// <summary>
/// A Data Transfer Object (DTO) for representing a quote, optimized for transport layers.
/// </summary>
/// <param name="Id">The unique identifier of the quote.</param>
/// <param name="AuthorId">The unique identifier of the associated author.</param>
/// <param name="Content">The text content of the quote.</param>
/// <param name="YearValue">The raw numerical value of the year.</param>
public record QuoteDto(int Id, string Content, string Author)
{
    public QuoteDto() : this(default, string.Empty, string.Empty) { }
}

