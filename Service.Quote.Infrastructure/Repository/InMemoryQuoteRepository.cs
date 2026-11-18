using System.Collections.Concurrent;
using Service.Quote.Domain.Repository;

namespace Service.Quote.Infrastructure.Repository;

public class InMemoryQuoteRepository : IQuoteRepository
{
    private readonly ConcurrentDictionary<int, Domain.Model.Quote> _quotes = new();
    private int _nextId;

    public InMemoryQuoteRepository()
    {
    }

    public InMemoryQuoteRepository(IEnumerable<Domain.Model.Quote> initialQuotes)
    {
        ArgumentNullException.ThrowIfNull(initialQuotes);

        foreach (var quote in initialQuotes)
        {
            Add(quote);
        }
    }

    public int Add(Domain.Model.Quote quote)
    {
        ArgumentNullException.ThrowIfNull(quote);

        var id = Interlocked.Increment(ref _nextId);
        _quotes[id] = quote;
        return id;
    }

    public Task<Domain.Model.Quote> Get(int id)
    {
        if (_quotes.TryGetValue(id, out var quote))
        {
            return Task.FromResult(quote);
        }

        throw new KeyNotFoundException($"Quote with id '{id}' was not found.");
    }

    public Task<int> Count()
    {
        return Task.FromResult(_quotes.Count);
    }

    public Task<IEnumerable<Domain.Model.Quote>> GetAllOfAuthor(int authorId)
    {
        IEnumerable<Domain.Model.Quote> result = _quotes.Values
            .Where(q => q.Author.Id == authorId)
            .ToList();

        return Task.FromResult(result);
    }
}