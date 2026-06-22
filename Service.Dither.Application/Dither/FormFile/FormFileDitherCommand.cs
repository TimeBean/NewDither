using MediatR;
using Microsoft.AspNetCore.Http;
using Service.Dither.Core.Model.Processor;
using Service.Dither.Core.Model.Quantizer;
using SkiaSharp;

namespace Service.Dither.Application.Dither.FormFile;

/// <summary>
/// Represents a command for dithering an image represented as an array of pixels.
/// </summary>
/// <param name="File">
/// A FormFile that represents image.
/// </param>
/// <param name="Processor">
/// The dithering processor that defines the algorithm used to process the image.
/// </param>
/// <param name="Quantizer">
/// The color quantizer responsible for reducing the number of colors in the image.
/// </param>
public record FormFileDitherCommand(IFormFile File, IQuantizer Quantizer, IProcessor Processor) : IRequest<SKData>;
