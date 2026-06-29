namespace Service.WebSite.Domain.Service;

public interface IQuoteService
{
    Task<Quote.Domain.Model.Quote?> GetRandom();
    Task<Quote.Domain.Model.Quote?> GetById(int id);
}