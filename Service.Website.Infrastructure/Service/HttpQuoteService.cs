using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Service.WebSite.Domain.Service;

namespace Service.Website.Infrastructure.Service;

public class HttpQuoteService : IQuoteService
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web)
    {
        Converters = { new JsonStringEnumConverter() }
    };

    private readonly HttpClient _httpClient;

    public HttpQuoteService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<Quote.Domain.Model.Quote?> GetRandom()
    {
        return await _httpClient.GetFromJsonAsync<Quote.Domain.Model.Quote>("quote/random", JsonOptions);
    }

    public async Task<Quote.Domain.Model.Quote?> GetById(int id)
    {
        return await _httpClient.GetFromJsonAsync<Quote.Domain.Model.Quote>($"quote/{id}", JsonOptions);
    }
}
