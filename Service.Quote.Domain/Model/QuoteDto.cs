namespace Service.Quote.Domain.Model;

public record QuoteDto(int Id, int AuthorId, string Content, int YearValue);