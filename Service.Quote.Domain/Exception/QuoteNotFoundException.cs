namespace Service.Quote.Domain.Exception;

public class QuoteNotFoundException : System.Exception
{
    public QuoteNotFoundException()
    {
    }

    public QuoteNotFoundException(string? message) : base(message)
    {
    }
}