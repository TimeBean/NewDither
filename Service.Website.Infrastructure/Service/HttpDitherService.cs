using System.Net.Http.Headers;
using Service.WebSite.Domain;
using Service.WebSite.Domain.Service;

namespace Service.Website.Infrastructure.Service;

public class HttpDitherService : IDitherService
{
    private readonly HttpClient _httpClient;

    public HttpDitherService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<byte[]?> ProcessAsync(Stream fileStream, string fileName, string contentType,
        DitherAlgorithm ditherAlgorithm, QuantizationAlgorithm quantizationAlgorithm)
    {
        var ditherAlg = Uri.EscapeDataString(ditherAlgorithm.ToString());
        var quantAlg = Uri.EscapeDataString(quantizationAlgorithm.ToString());
        var requestUrl = $"/dither?ditherAlgorithm={ditherAlg}&quantizationAlgorithm={quantAlg}";

        using var content = new MultipartFormDataContent();

        var fileContent = new StreamContent(fileStream);
        fileContent.Headers.ContentType = new MediaTypeHeaderValue(contentType);

        content.Add(fileContent, "file", fileName);

        var response = await _httpClient.PostAsync(requestUrl, content);

        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        return await response.Content.ReadAsByteArrayAsync();
    }
}
