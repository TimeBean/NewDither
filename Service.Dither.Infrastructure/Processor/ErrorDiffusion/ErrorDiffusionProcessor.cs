using Service.Dither.Core.Processor;
using Service.Dither.Core.Quantizer;

namespace Service.Dither.Infrastructure.Processor.ErrorDiffusion;

/// <summary>
/// Provides a base implementation for error diffusion dithering processors.
/// </summary>
public abstract class ErrorDiffusionProcessor : IProcessor
{
    /// <summary>
    /// Distributes the quantization error to neighboring pixels based on a specific error diffusion algorithm.
    /// </summary>
    /// <param name="pixels">The span containing the image pixel data.</param>
    /// <param name="x">The X-coordinate of the current pixel.</param>
    /// <param name="y">The Y-coordinate of the current pixel.</param>
    /// <param name="channel">The specific color channel index being processed.</param>
    /// <param name="error">The quantization error value to distribute.</param>
    protected abstract void DistributeError(byte[] pixels, int x, int y, int channel, double error);
    
    /// <summary>
    /// Initializes a new instance of the <see cref="ErrorDiffusionProcessor"/> class with image dimensions and byte layout.
    /// </summary>
    /// <param name="width">The width of the image in pixels.</param>
    /// <param name="height">The height of the image in pixels.</param>
    /// <param name="rowBytes">The number of bytes per image row (stride).</param>
    /// <param name="bytesPerPixel">The number of bytes used per single pixel.</param>
    protected ErrorDiffusionProcessor(int width, int height, int rowBytes, int bytesPerPixel)
    {
        Width = width;
        Height = height;
        RowBytes = rowBytes;
        BytesPerPixel = bytesPerPixel;
    }

    /// <summary>
    /// Gets the width of the image in pixels.
    /// </summary>
    public int Width { get; protected init; }

    /// <summary>
    /// Gets the height of the image in pixels.
    /// </summary>
    public int Height { get; protected init; }

    /// <summary>
    /// Gets the number of bytes per image row (stride).
    /// </summary>
    public int RowBytes { get; protected init; }

    /// <summary>
    /// Gets the number of bytes used per single pixel.
    /// </summary>
    public int BytesPerPixel { get; protected init; }

    /// <summary>
    /// Processes the image data by applying color quantization and diffusing the resulting error to adjacent pixels.
    /// </summary>
    /// <param name="pixels">A reference to the span containing the image pixel bytes to be modified in-place.</param>
    /// <param name="quantizer">The quantizer used to map original colors to the target color palette.</param>
    public void Process(byte[] pixels, IQuantizer quantizer)
    {
        for (var y = 0; y < Height; y++)
        {
            for (var x = 0; x < Width; x++)
            {
                var baseIndex = y * RowBytes + x * BytesPerPixel;

                var newValues = new List<float>();

                for (var c = 0; c < 3; c++)
                {
                    newValues.Add(pixels[baseIndex + c]);
                }

                var quantizedColors = quantizer.Quantize(newValues.ToArray());

                for (var c = 0; c < 3; c++)
                {
                    var index = baseIndex + c;

                    var oldValue = pixels[index];
                    var newValue = quantizedColors[c];

                    var error = oldValue - newValue;
                    pixels[index] = (byte)Math.Round(newValue);

                    DistributeError(pixels, x, y, c, error);
                }
            }
        }
    }
}