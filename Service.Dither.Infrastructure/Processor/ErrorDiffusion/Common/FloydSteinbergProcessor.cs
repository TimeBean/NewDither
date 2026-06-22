namespace Service.Dither.Infrastructure.Processor.ErrorDiffusion.Common;

/// <summary>
/// Implements the classic Floyd-Steinberg error diffusion dithering algorithm.
/// </summary>
public sealed class FloydSteinbergProcessor : ErrorDiffusionProcessor
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FloydSteinbergProcessor"/> class with image dimensions and byte layout.
    /// </summary>
    /// <param name="width">The width of the image in pixels.</param>
    /// <param name="height">The height of the image in pixels.</param>
    /// <param name="rowBytes">The number of bytes per image row (stride).</param>
    /// <param name="bytesPerPixel">The number of bytes used per single pixel.</param>
    public FloydSteinbergProcessor(int width, int height, int rowBytes, int bytesPerPixel)
        : base(width, height, rowBytes, bytesPerPixel)
    {
    }

    /// <summary>
    /// Distributes the quantization error to neighboring pixels using the Floyd-Steinberg coefficients:
    /// Right (7/16), Bottom-Left (3/16), Bottom (5/16), and Bottom-Right (1/16).
    /// </summary>
    /// <param name="pixels">The span containing the image pixel data.</param>
    /// <param name="x">The X-coordinate of the current pixel.</param>
    /// <param name="y">The Y-coordinate of the current pixel.</param>
    /// <param name="channel">The specific color channel index being processed.</param>
    /// <param name="error">The quantization error value to distribute.</param>
    protected override void DistributeError(byte[] pixels, int x, int y, int channel, double error)
    {
        Add(pixels, x + 1, y, channel, error, 7.0 / 16.0);
        Add(pixels, x - 1, y + 1, channel, error, 3.0 / 16.0);
        Add(pixels, x, y + 1, channel, error, 5.0 / 16.0);
        Add(pixels, x + 1, y + 1, channel, error, 1.0 / 16.0);
    }

    /// <summary>
    /// Applies a fraction of the error to a target neighbor pixel, ensuring bounds checking and value clamping.
    /// </summary>
    /// <param name="pixels">The span containing the image pixel data.</param>
    /// <param name="nx">The X-coordinate of the target neighbor pixel.</param>
    /// <param name="ny">The Y-coordinate of the target neighbor pixel.</param>
    /// <param name="channel">The specific color channel index being processed.</param>
    /// <param name="error">The total quantization error value from the source pixel.</param>
    /// <param name="factor">The weight factor applied to the distributed error.</param>
    private void Add(Span<byte> pixels, int nx, int ny, int channel, double error, double factor)
    {
        if (nx < 0 || nx >= Width || ny < 0 || ny >= Height)
            return;

        int idx = ny * RowBytes + nx * BytesPerPixel + channel;

        if ((uint)idx >= (uint)pixels.Length)
            return;

        double value = pixels[idx] + error * factor;

        if (value < 0) value = 0;
        else if (value > 255) value = 255;

        pixels[idx] = (byte)Math.Round(value);
    }
}