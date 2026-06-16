namespace Service.Dither.Infrastructure.Processor.ErrorDiffusion.Common;

/// <summary>
/// Implements the Burkes error diffusion dithering algorithm.
/// </summary>
public sealed class BurkesProcessor : ErrorDiffusionProcessor
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BurkesProcessor"/> class with image dimensions and byte layout.
    /// </summary>
    /// <param name="width">The width of the image in pixels.</param>
    /// <param name="height">The height of the image in pixels.</param>
    /// <param name="rowBytes">The number of bytes per image row (stride).</param>
    /// <param name="bytesPerPixel">The number of bytes used per single pixel.</param>
    public BurkesProcessor(int width, int height, int rowBytes, int bytesPerPixel)
        : base(width, height, rowBytes, bytesPerPixel)
    {
    }

    /// <summary>
    /// Distributes the quantization error to neighboring pixels using the Burkes coefficients:
    /// Same row: Right (+1) with 8/32, Right (+2) with 4/32.
    /// Next row: Left (-2) with 2/32, Left (-1) with 4/32, Straight down (0) with 8/32, Right (+1) with 4/32, and Right (+2) with 2/32.
    /// </summary>
    /// <param name="pixels">The span containing the image pixel data.</param>
    /// <param name="x">The X-coordinate of the current pixel.</param>
    /// <param name="y">The Y-coordinate of the current pixel.</param>
    /// <param name="channel">The specific color channel index being processed.</param>
    /// <param name="error">The quantization error value to distribute.</param>
    protected override void DistributeError(byte[] pixels, int x, int y, int channel, double error)
    {
        Add(pixels, x + 1, y, channel, error, 8.0 / 32.0);
        Add(pixels, x + 2, y, channel, error, 4.0 / 32.0);

        Add(pixels, x - 2, y + 1, channel, error, 2.0 / 32.0);
        Add(pixels, x - 1, y + 1, channel, error, 4.0 / 32.0);
        Add(pixels, x, y + 1, channel, error, 8.0 / 32.0);
        Add(pixels, x + 1, y + 1, channel, error, 4.0 / 32.0);
        Add(pixels, x + 2, y + 1, channel, error, 2.0 / 32.0);
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
        {
            return;
        }

        var idx = ny * RowBytes + nx * BytesPerPixel + channel;
        var value = pixels[idx] + error * factor;

        value = value switch
        {
            < 0 => 0,
            > 255 => 255,
            _ => value
        };

        pixels[idx] = (byte)Math.Round(value);
    }
}