using Service.WebSite.Domain.Service;

namespace Service.WebSite.Presentation.Mvc.Models;

public class IndexModel
{
    public Quote.Domain.Model.Quote RandomQuote { get; }

    public IndexModel(Quote.Domain.Model.Quote randomQuote)
    {
        RandomQuote = randomQuote;
    }
}