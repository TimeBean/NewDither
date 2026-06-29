using MediatR;
using Microsoft.AspNetCore.Http;
using Service.Dither.Core.Model;
using SkiaSharp;

namespace Service.Dither.Application.Dither.Request;

public record DitherRequestCommand(
    IFormFile File,
    DitherAlgorithm DitherAlgorithm,
    QuantizationAlgorithm QuantizeAlgorithm) : IRequest<SKData>;