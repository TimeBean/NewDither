using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Service.WebSite.Domain.Service;
using Service.WebSite.Presentation.Mvc.Models;

namespace Service.WebSite.Presentation.Mvc.Controllers;

public class HomeController : Controller
{
    private readonly IDitherService _ditherService;
    private readonly IQuoteService _quoteService;

    public HomeController(IDitherService ditherService, IQuoteService quoteService)
    {
        _ditherService = ditherService;
        _quoteService = quoteService;
    }
    
    [HttpPost("/dither/process")]
    public async Task<IActionResult> ProcessAjax([FromForm] DitherProcessRequest request)
    {
        var quote = await _quoteService.GetRandom();
        if (quote == null)
        {
            throw new Exception("Quote not found.");
        }

        if (request.File.Length == 0)
        {
            return View("Index", new IndexModel(quote, errorMessage: "File not selected or empty."));
        }

        var stopwatch = Stopwatch.StartNew();

        try
        {
            var imageBytes = await _ditherService.ProcessAsync(
                request.File.OpenReadStream(),
                request.File.FileName,
                request.File.ContentType,
                request.DitherAlgorithm,
                request.QuantizationAlgorithm);

            if (imageBytes == null)
            {
                return View("Index", new IndexModel(quote,
                    errorMessage: "Error on remote dithering server.",
                    fileName: request.File.FileName,
                    selectedDitherAlgorithm: request.DitherAlgorithm,
                    selectedQuantizationAlgorithm: request.QuantizationAlgorithm));
            }

            stopwatch.Stop();

            return View("Index", new IndexModel(quote,
                resultImageBase64: Convert.ToBase64String(imageBytes),
                executionTime: stopwatch.ElapsedMilliseconds,
                fileName: $"dithered-{request.File.FileName}",
                selectedDitherAlgorithm: request.DitherAlgorithm,
                selectedQuantizationAlgorithm: request.QuantizationAlgorithm));
        }
        catch (Exception ex)
        {
            return View("Index", new IndexModel(quote,
                errorMessage: $"Internal server error: {ex.Message}",
                fileName: request.File.FileName,
                selectedDitherAlgorithm: request.DitherAlgorithm,
                selectedQuantizationAlgorithm: request.QuantizationAlgorithm));
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