using System.Text.Json.Serialization;
using MediatR;
using Scalar.AspNetCore;
using Service.Dither.Application.Dither.Common;
using Service.Dither.Core.Model;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblyContaining<CommonDitherHandler>();
});

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(
        new JsonStringEnumConverter());
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.MapGet("/dither/debug", (DitherAlgorithm ditherAlgorithm, QuantizationAlgorithm quantizationAlgorithm) =>
    Results.Ok(new { ditherAlgorithm = ditherAlgorithm.ToString(), ditherAlgorithmInt = (int)ditherAlgorithm, quantizationAlgorithm = quantizationAlgorithm.ToString(), quantizationAlgorithmInt = (int)quantizationAlgorithm }));

app.MapPost("/dither", async (IMediator mediator,
        IFormFile file, DitherAlgorithm ditherAlgorithm, QuantizationAlgorithm quantizationAlgorithm,
        HttpContext httpContext, ILogger<Program> logger) =>
    {
        logger.LogWarning("DITHER API: ditherAlgorithm={Algo} (int={(int)ditherAlgorithm}), quantizeAlgorithm={Quant} (int={(int)quantizationAlgorithm})",
            ditherAlgorithm, ditherAlgorithm, quantizationAlgorithm, quantizationAlgorithm);

        await using var stream = file.OpenReadStream();
        using var ms = new MemoryStream();
        await stream.CopyToAsync(ms);

        if (ms.Length == 0)
            return Results.BadRequest("Empty file");

        using var data =
            await mediator.Send(new DitherCommand(ms.ToArray(), ditherAlgorithm, quantizationAlgorithm));

        return Results.File(data.ToArray(), "image/png");
    })
    .DisableAntiforgery();

app.Run();