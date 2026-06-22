using Service.Quote.Domain.Model.Numeric;

namespace Service.Quote.Domain.Model;

public record Quote(Author Author, string Content, Year Year);