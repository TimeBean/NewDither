using System.Diagnostics;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Service.Dither.Application.Dither;
using Service.Dither.Core.Processor;
using Service.Dither.Core.Quantizer;
using Service.Dither.Infrastructure.Processor.ErrorDiffusion.Common;
using Service.Dither.Infrastructure.Quantizer;
using SkiaSharp;

namespace Service.Dither.Presentation.Cli;

internal class Program
{
    static async Task Main(string[] args)
    {
        var services = new ServiceCollection();

        services.AddLogging();
        
        services.AddMediatR(cfg => 
        {
            cfg.RegisterServicesFromAssembly(typeof(DitherCommand).Assembly);
        });

        var serviceProvider = services.BuildServiceProvider();
        var mediator = serviceProvider.GetRequiredService<IMediator>();

        using var input = File.OpenRead(@"Examples/image.png");
        using var originalBitmap = SKBitmap.Decode(input);
    
        var width = originalBitmap.Width;
        var height = originalBitmap.Height;
        var rowBytes = originalBitmap.RowBytes;
        var bpp = originalBitmap.BytesPerPixel;
    
        /*IQuantizer quantizer = new LinearQuantizer(2);*/
        IQuantizer quantizer = new LinearQuantizer(2);
        IProcessor processor = new OptimizedFloydSteinbergProcessor(width, height, rowBytes, bpp);
        /*IProcessor processor = new FloydSteinbergProcessor(width, height, rowBytes, bpp);*/
    
        byte[] pixelBytes = originalBitmap.Bytes;
    
        var processStopwatch = Stopwatch.StartNew();
        await mediator.Send(new DitherCommand(pixelBytes, processor, quantizer));
        processStopwatch.Stop();
        
        System.Runtime.InteropServices.Marshal.Copy(pixelBytes, 0, originalBitmap.GetPixels(), pixelBytes.Length);

        using var image = SKImage.FromBitmap(originalBitmap);
        using var data = image.Encode(SKEncodedImageFormat.Png, 100);
    
        using var output = File.OpenWrite(@"dither.png");
        data.SaveTo(output);
        
        Console.WriteLine("Изображение успешно обработано и сохранено!\n" +
                          $"{processStopwatch.ElapsedMilliseconds}ms");
    }
}