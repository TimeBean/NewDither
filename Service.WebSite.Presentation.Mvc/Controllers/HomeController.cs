using System.Diagnostics;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Service.WebSite.Domain.Service;
using Service.WebSite.Presentation.Mvc.Models;

namespace Service.WebSite.Presentation.Mvc.Controllers;

public class HomeController : Controller
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly IQuoteService _quoteService;

    public HomeController(IHttpClientFactory clientFactory, IQuoteService quoteService)
    {
        _clientFactory = clientFactory;
        _quoteService = quoteService;
    }
    
    [HttpPost("/dither/process")]
    public async Task<IActionResult> ProcessAjax([FromForm] DitherProcessRequest request)
    {
        if (request.File.Length == 0)
        {
            return BadRequest(new { message = "Файл не выбран или пуст." });
        }

        var stopwatch = Stopwatch.StartNew();

        try
        {
            var client = _clientFactory.CreateClient("DitherApiClient");

            var ditherAlg = Uri.EscapeDataString(request.DitherAlgorithm.ToString());
            var quantAlg = Uri.EscapeDataString(request.QuantizationAlgorithm.ToString());
            var requestUrl = $"/dither?ditherAlgorithm={ditherAlg}&quantizationAlgorithm={quantAlg}";

            using var content = new MultipartFormDataContent();

            var fileContent = new StreamContent(request.File.OpenReadStream());
            fileContent.Headers.ContentType = new MediaTypeHeaderValue(request.File.ContentType);

            content.Add(fileContent, "file", request.File.FileName);

            var response = await client.PostAsync(requestUrl, content);

            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode, new { message = "Ошибка на удаленном сервере дизеринга." });
            }

            var imageBytes = await response.Content.ReadAsByteArrayAsync();

            stopwatch.Stop();

            return Json(new
            {
                imageBase64 = Convert.ToBase64String(imageBytes),
                executionTime = stopwatch.ElapsedMilliseconds
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = $"Внутренняя ошибка сервера: {ex.Message}" });
        }
    }
    
    public async Task<IActionResult> Index()
    {
        var quote = await _quoteService.GetRandom();

        if (quote == null)
        {
            throw new Exception("Quote not found.");
        }
        
        var model = new IndexModel(quote);

        return View(model);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}