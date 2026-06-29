using System.Runtime.InteropServices;
using MediatR;
using Service.Dither.Core.Model;
using Service.Dither.Core.Model.Processor;
using Service.Dither.Core.Model.Quantizer;
using Service.Dither.Infrastructure.Processor.ErrorDiffusion.Common;
using Service.Dither.Infrastructure.Processor.ErrorDiffusion.LongRange;
using Service.Dither.Infrastructure.Processor.ErrorDiffusion.Special;
using Service.Dither.Infrastructure.Processor.Ordered;
using Service.Dither.Infrastructure.Processor.Ordered.Constants;
using Service.Dither.Infrastructure.Processor.Special;
using Service.Dither.Infrastructure.Quantizer;
using SkiaSharp;

namespace Service.Dither.Application.Dither.Common;

public class CommonDitherHandler : IRequestHandler<DitherCommand, SKData>
{
    public async Task<SKData> Handle(DitherCommand request, CancellationToken cancellationToken)
    {
        IQuantizer quantizer = request.QuantizeAlgorithm switch
        {
            QuantizationAlgorithm.Linear2 => new LinearQuantizer(2),
            QuantizationAlgorithm.Linear4 => new LinearQuantizer(4),
            QuantizationAlgorithm.Linear8 => new LinearQuantizer(8),
            QuantizationAlgorithm.Linear16 => new LinearQuantizer(16),
            QuantizationAlgorithm.Linear32 => new LinearQuantizer(32),
            QuantizationAlgorithm.Linear64 => new LinearQuantizer(64),
            QuantizationAlgorithm.GrayscaleByAverage => new GrayscaleByAverageQuantizer(),

            _ => throw new ArgumentOutOfRangeException()
        };

        using var ms = new MemoryStream(request.ImageBytes);
        using var original = SKBitmap.Decode(ms);
        if (original == null) throw new InvalidOperationException("Could not load bitmap");

        var isConverted = original.ColorType != SKColorType.Bgra8888;
        using var bitmap = isConverted
            ? ConvertToBgra8888(original)
            : original;

        var width = bitmap.Width;
        var height = bitmap.Height;
        var rowBytes = bitmap.RowBytes;
        var bytesPerPixel = bitmap.BytesPerPixel;

        IProcessor processor = request.DitherAlgorithm switch
        {
            DitherAlgorithm.QuantizeOnly =>
                new QuantizeOnlyProcessor(width, height, rowBytes, bytesPerPixel),

            DitherAlgorithm.Burkes =>
                new BurkesProcessor(width, height, rowBytes, bytesPerPixel),

            DitherAlgorithm.FloydSteinberg =>
                new FloydSteinbergProcessor(width, height, rowBytes, bytesPerPixel),

            DitherAlgorithm.JarvisJudiceNinke =>
                new JarvisJudiceNinkeProcessor(width, height, rowBytes, bytesPerPixel),

            DitherAlgorithm.Sierra3 =>
                new Sierra3Processor(width, height, rowBytes, bytesPerPixel),

            DitherAlgorithm.Stucki =>
                new StuckiProcessor(width, height, rowBytes, bytesPerPixel),

            DitherAlgorithm.Atkinson =>
                new AtkinsonProcessor(width, height, rowBytes, bytesPerPixel),

            DitherAlgorithm.OrderedBayer2X2 =>
                new OrderedProcessor(width, height, rowBytes, bytesPerPixel,
                    Bayer.Bayer2X2),

            DitherAlgorithm.OrderedBayer4X4 =>
                new OrderedProcessor(width, height, rowBytes, bytesPerPixel,
                    Bayer.Bayer4X4),

            DitherAlgorithm.OrderedBayer8X8 =>
                new OrderedProcessor(width, height, rowBytes, bytesPerPixel,
                    Bayer.Bayer8X8),

            DitherAlgorithm.OrderedCluster4X4 =>
                new OrderedProcessor(width, height, rowBytes, bytesPerPixel,
                    Cluster.Cluster4X4),

            DitherAlgorithm.OrderedCluster8X8 =>
                new OrderedProcessor(width, height, rowBytes, bytesPerPixel,
                    Cluster.Cluster8X8),

            DitherAlgorithm.OrderedHalftone4X4 =>
                new OrderedProcessor(width, height, rowBytes, bytesPerPixel,
                    Halftone.Halftone4),

            DitherAlgorithm.OrderedHalftone8X8 =>
                new OrderedProcessor(width, height, rowBytes, bytesPerPixel,
                    Halftone.Halftone8),

            _ => throw new ArgumentOutOfRangeException(
                nameof(request.DitherAlgorithm),
                request.DitherAlgorithm,
                "Unknown dithering algorithm")
        };

        var pixels = bitmap.GetPixels();
        var length = bitmap.ByteCount;
        var buffer = new byte[length];
        Marshal.Copy(pixels, buffer, 0, length);
        processor.Process(buffer, quantizer);
        Marshal.Copy(buffer, 0, pixels, length);

        using var image = SKImage.FromBitmap(bitmap);
        var data = image.Encode(SKEncodedImageFormat.Png, 100);

        return data;
    }

    private static SKBitmap ConvertToBgra8888(SKBitmap source)
    {
        var converted = new SKBitmap(source.Width, source.Height, SKColorType.Bgra8888, SKAlphaType.Premul);
        if (!source.CopyTo(converted, SKColorType.Bgra8888))
        {
            throw new InvalidOperationException("Could not convert bitmap to BGRA 8888");
        }
        return converted;
    }
}
