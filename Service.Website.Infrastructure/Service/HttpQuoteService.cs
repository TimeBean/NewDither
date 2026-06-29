using System.Net.Http.Json;
using Service.WebSite.Domain.Service;

namespace Service.Website.Infrastructure.Service;

public class HttpQuoteService : IQuoteService
{
    private readonly HttpClient _httpClient;

    public HttpQuoteService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<Quote.Domain.Model.Quote?> GetRandom()
    {
        return await _httpClient.GetFromJsonAsync<Quote.Domain.Model.Quote>("quote/random");
    }

    public async Task<Quote.Domain.Model.Quote?> GetById(int id)
    {
        return await _httpClient.GetFromJsonAsync<Quote.Domain.Model.Quote>($"quote/{id}");
    }
}