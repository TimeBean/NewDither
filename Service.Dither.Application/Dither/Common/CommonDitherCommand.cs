using MediatR;
using Service.Dither.Core.Model.Processor;
using Service.Dither.Core.Model.Quantizer;

namespace Service.Dither.Application.Dither.Common;

/// <summary>
/// Represents a command for dithering an image represented as an array of pixels.
/// </summary>
/// <param name="Pixels">
/// A byte array representing RGB pixels. Each pixel consists of three consecutive bytes:
/// red, green, and blue components.
/// </param>
/// <param name="Processor">
/// The dithering processor that defines the algorithm used to process the image.
/// </param>
/// <param name="Quantizer">
/// The color quantizer responsible for reducing the number of colors in the image.
/// </param>
public record CommonDitherCommand(byte[] Pixels, IProcessor Processor, IQuantizer Quantizer) : IRequest;