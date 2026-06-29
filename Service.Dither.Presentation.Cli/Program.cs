using System.Diagnostics;
using System.Runtime.InteropServices;
using Service.Dither.Core.Model.Processor;
using Service.Dither.Core.Model.Quantizer;
using Service.Dither.Infrastructure.Processor.ErrorDiffusion.Common;
using Service.Dither.Infrastructure.Quantizer;
using SkiaSharp;

using var input = File.OpenRead(@"Examples/image.png");
using var originalBitmap = SKBitmap.Decode(input);

var width = originalBitmap.Width;
var height = originalBitmap.Height;
var rowBytes = originalBitmap.RowBytes;
var bpp = originalBitmap.BytesPerPixel;

IQuantizer quantizer = new LinearQuantizer(2);
IProcessor processor = new OptimizedFloydSteinbergProcessor(width, height, rowBytes, bpp);

byte[] pixelBytes = originalBitmap.Bytes;

var processStopwatch = Stopwatch.StartNew();
processor.Process(pixelBytes, quantizer);
processStopwatch.Stop();

Marshal.Copy(pixelBytes, 0, originalBitmap.GetPixels(), pixelBytes.Length);

using var image = SKImage.FromBitmap(originalBitmap);
using var data = image.Encode(SKEncodedImageFormat.Png, 100);

using var output = File.OpenWrite(@"dither.png");
data.SaveTo(output);

Console.WriteLine("Изображение успешно обработано и сохранено!\n" +
                  $"{processStopwatch.ElapsedMilliseconds}ms");
