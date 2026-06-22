using MediatR;
using SkiaSharp;

namespace Service.Dither.Application.GetData;

public class GetDataHandler : IRequestHandler<GetDataCommand, Data>
{
    public async Task<Data> Handle(GetDataCommand request, CancellationToken cancellationToken)
    {
        await using var stream = request.File.OpenReadStream();

        using var bitmap = SKBitmap.Decode(stream);

        if (bitmap == null)
        {
            throw new InvalidOperationException("Could not load bitmap");
        }

        var width = bitmap.Width;
        var height = bitmap.Height;

        var rowBytes = bitmap.RowBytes;
        var bytesPerPixel = bitmap.BytesPerPixel;
        
        return new Data(width, height, rowBytes, bytesPerPixel);
    }
}