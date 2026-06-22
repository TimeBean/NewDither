namespace Service.Quote.Domain.Repository;

public interface IQuoteRepository
{
    Task<Model.Quote> Get(int id);
    Task<int> Count();
    Task<IEnumerable<Model.Quote>> GetAllOfAuthor(int authorId);
}