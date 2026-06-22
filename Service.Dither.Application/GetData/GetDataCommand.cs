using MediatR;
using Microsoft.AspNetCore.Http;

namespace Service.Dither.Application.GetData;

public record Data(int Width, int Height, int RowBytes, int BytesPerPixel);

public record GetDataCommand(IFormFile File)
    : IRequest<Data>;