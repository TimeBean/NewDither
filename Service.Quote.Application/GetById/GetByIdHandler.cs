using MediatR;
using Service.Quote.Domain.Repository;

namespace Service.Quote.Application.GetById;

public class GetByIdHandler : IRequestHandler<GetByIdCommand, Domain.Model.Quote>
{
    private readonly IQuoteRepository _quoteRepository;

    public GetByIdHandler(IQuoteRepository quoteRepository)
    {
        _quoteRepository = quoteRepository;
    }

    public async Task<Domain.Model.Quote> Handle(GetByIdCommand request, CancellationToken cancellationToken)
    {
        return await _quoteRepository.Get(request.Id);
    }
}