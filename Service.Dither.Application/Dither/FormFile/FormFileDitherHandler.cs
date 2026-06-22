using System.Runtime.InteropServices;
using MediatR;
using Service.Dither.Application.Dither.Common;
using Service.Dither.Core.Model.Processor;
using Service.Dither.Core.Model.Quantizer;
using Service.Dither.Infrastructure.Processor.ErrorDiffusion.Common;
using Service.Dither.Infrastructure.Quantizer;
using SkiaSharp;

namespace Service.Dither.Application.Dither.FormFile;

public class FormFileDitherHandler : IRequestHandler<FormFileDitherCommand, SKData>
{
    private readonly IMediator _mediator;
    
    public FormFileDitherHandler(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    public async Task<SKData> Handle(FormFileDitherCommand request, CancellationToken cancellationToken)
    {
        await using var stream = request.File.OpenReadStream();

        using var bitmap = SKBitmap.Decode(stream);

        if (bitmap == null)
        {
            throw new InvalidOperationException("Could not load bitmap");
        }

        var width = bitmap.Width;
        var height = bitmap.Height;

        var pixels = bitmap.GetPixels();
        var length = bitmap.ByteCount;

        var buffer = new byte[length];
        Marshal.Copy(pixels, buffer, 0, length);

        await _mediator.Send(new CommonDitherCommand(buffer, request.Processor, request.Quantizer));

        Marshal.Copy(buffer, 0, pixels, length);

        using var image = SKImage.FromBitmap(bitmap);
        var data = image.Encode(SKEncodedImageFormat.Png, 100);

        return data;
    }
}