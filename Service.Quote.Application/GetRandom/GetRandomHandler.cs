using MediatR;
using Service.Quote.Domain.Exception;
using Service.Quote.Domain.Repository;

namespace Service.Quote.Application.GetRandom;

public class GetRandomHandler : IRequestHandler<GetRandomCommand, Domain.Model.Quote>
{
    private readonly IQuoteRepository _quoteRepository;
    
    public GetRandomHandler(IQuoteRepository quoteRepository)
    {
        _quoteRepository = quoteRepository;
    }
    
    public async Task<Domain.Model.Quote> Handle(GetRandomCommand request, CancellationToken cancellationToken)
    {
        var random = new Random();
        
        var quotesCount = await _quoteRepository.Count();
        if (quotesCount <= 0) throw new RepositoryContainsNoQuotesException();
        var id = random.Next(1, quotesCount);
        
        return await _quoteRepository.Get(id);
    }
}