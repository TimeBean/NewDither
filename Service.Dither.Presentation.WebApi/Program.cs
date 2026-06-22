using System.Text.Json.Serialization;
using MediatR;
using Scalar.AspNetCore;
using Service.Dither.Application.Dither.FormFile;
using Service.Dither.Application.Dither.Request;
using Service.Dither.Core.Model;
using SkiaSharp;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();

builder.Services.AddMediatR(cfg => { cfg.RegisterServicesFromAssemblyContaining<FormFileDitherCommand>(); });

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

app.MapPost("/dither", async (IMediator mediator,
        IFormFile file, DitherAlgorithm ditherAlgorithm, QuantizationAlgorithm quantizationAlgorithm) =>
    {
        await using var stream = file.OpenReadStream();
        using var bitmap = SKBitmap.Decode(stream);
        if (bitmap == null)
        {
            return Results.BadRequest("Invalid image");
        }

        using var data =
            await mediator.Send(new DitherRequestCommand(file, ditherAlgorithm, quantizationAlgorithm));

        return Results.File(data.ToArray(), "image/png");
    })
    .DisableAntiforgery();

app.Run();