using MediatR;
using Service.Dither.Core.Model;
using SkiaSharp;

namespace Service.Dither.Application.Dither.Common;

public record DitherCommand(
    byte[] ImageBytes,
    DitherAlgorithm DitherAlgorithm,
    QuantizationAlgorithm QuantizeAlgorithm) : IRequest<SKData>;