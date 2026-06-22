using MediatR;
using Service.Dither.Application.Dither.FormFile;
using Service.Dither.Application.GetData;
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

namespace Service.Dither.Application.Dither.Request;

public class DitherRequestHandler : IRequestHandler<DitherRequestCommand, SKData>
{
    private readonly IMediator _mediator;

    public DitherRequestHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<SKData> Handle(DitherRequestCommand request, CancellationToken cancellationToken)
    {
        IQuantizer quantizer = request.QuantizeAlgorithm switch
        {
            QuantizationAlgorithm.Linear2 => new LinearQuantizer(2),
            QuantizationAlgorithm.Linear4 => new LinearQuantizer(4),
            QuantizationAlgorithm.Linear8 => new LinearQuantizer(8),
            QuantizationAlgorithm.Linear16 => new LinearQuantizer(16),
            QuantizationAlgorithm.Linear32 => new LinearQuantizer(32),
            QuantizationAlgorithm.Linear64 => new LinearQuantizer(64),

            _ => throw new ArgumentOutOfRangeException()
        };

        var data = await _mediator.Send(new GetDataCommand(request.File));
        IProcessor processor = request.DitherAlgorithm switch
        {
            DitherAlgorithm.QuantizeOnly =>
                new QuantizeOnlyProcessor(data.Width, data.Height, data.RowBytes, data.BytesPerPixel),

            DitherAlgorithm.Burkes =>
                new BurkesProcessor(data.Width, data.Height, data.RowBytes, data.BytesPerPixel),

            DitherAlgorithm.FloydSteinberg =>
                new FloydSteinbergProcessor(data.Width, data.Height, data.RowBytes, data.BytesPerPixel),

            DitherAlgorithm.JarvisJudiceNinke =>
                new JarvisJudiceNinkeProcessor(data.Width, data.Height, data.RowBytes, data.BytesPerPixel),

            DitherAlgorithm.Sierra3 =>
                new Sierra3Processor(data.Width, data.Height, data.RowBytes, data.BytesPerPixel),

            DitherAlgorithm.Stucki =>
                new StuckiProcessor(data.Width, data.Height, data.RowBytes, data.BytesPerPixel),

            DitherAlgorithm.Atkinson =>
                new AtkinsonProcessor(data.Width, data.Height, data.RowBytes, data.BytesPerPixel),

            DitherAlgorithm.OrderedBayer2X2 =>
                new OrderedProcessor(data.Width, data.Height, data.RowBytes, data.BytesPerPixel,
                    Bayer.Bayer2X2),

            DitherAlgorithm.OrderedBayer4X4 =>
                new OrderedProcessor(data.Width, data.Height, data.RowBytes, data.BytesPerPixel,
                    Bayer.Bayer4X4),

            DitherAlgorithm.OrderedBayer8X8 =>
                new OrderedProcessor(data.Width, data.Height, data.RowBytes, data.BytesPerPixel,
                    Bayer.Bayer8X8),

            DitherAlgorithm.OrderedCluster4X4 =>
                new OrderedProcessor(data.Width, data.Height, data.RowBytes, data.BytesPerPixel,
                    Cluster.Cluster4X4),

            DitherAlgorithm.OrderedCluster8X8 =>
                new OrderedProcessor(data.Width, data.Height, data.RowBytes, data.BytesPerPixel,
                    Cluster.Cluster8X8),

            DitherAlgorithm.OrderedHalftone4X4 =>
                new OrderedProcessor(data.Width, data.Height, data.RowBytes, data.BytesPerPixel,
                    Halftone.Halftone4),

            DitherAlgorithm.OrderedHalftone8X8 =>
                new OrderedProcessor(data.Width, data.Height, data.RowBytes, data.BytesPerPixel,
                    Halftone.Halftone4),

            _ => throw new ArgumentOutOfRangeException(
                nameof(request.DitherAlgorithm),
                request.DitherAlgorithm,
                "Unknown dithering algorithm")
        };

        return await _mediator.Send(new FormFileDitherCommand(request.File, quantizer, processor));
    }
}