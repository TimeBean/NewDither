using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Service.Dither.Infrastructure.Processor.ErrorDiffusion.Common;

/// <summary>
/// Implements a highly optimized classic Floyd-Steinberg error diffusion dithering algorithm.
/// </summary>
public sealed class OptimizedFloydSteinbergProcessor : ErrorDiffusionProcessor
{
    public OptimizedFloydSteinbergProcessor(int width, int height, int rowBytes, int bytesPerPixel)
        : base(width, height, rowBytes, bytesPerPixel)
    {
    }

    /// <summary>
    /// Distributes the quantization error to neighboring pixels using the Floyd-Steinberg coefficients.
    /// Optimized with Fast-Path/Slow-Path, Unsafe memory access, and branchless clamping.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected override void DistributeError(byte[] pixels, int x, int y, int channel, double error)
    {
        if (error == 0.0) return;

        int width = Width;
        int height = Height;
        int bpp = BytesPerPixel;
        int rowBytes = RowBytes;

        double factor16 = error * 0.0625; // 1.0 / 16.0
        double e7 = factor16 * 7.0;
        double e3 = factor16 * 3.0;
        double e5 = factor16 * 5.0;
        double e1 = factor16;

        ref byte pixelRef = ref MemoryMarshal.GetReference(pixels);

        int curRowOffset = y * rowBytes + channel;
        int nextRowOffset = curRowOffset + rowBytes;

        if (x > 0 && x < width - 1 && y < height - 1)
        {
            int idx;
            double v;

            // Right (x + 1, y)
            idx = curRowOffset + (x + 1) * bpp;
            v = Unsafe.Add(ref pixelRef, idx) + e7;
            Unsafe.Add(ref pixelRef, idx) = (byte)(Math.Clamp(v, 0.0, 255.0) + 0.5);

            // Bottom-Left (x - 1, y + 1)
            idx = nextRowOffset + (x - 1) * bpp;
            v = Unsafe.Add(ref pixelRef, idx) + e3;
            Unsafe.Add(ref pixelRef, idx) = (byte)(Math.Clamp(v, 0.0, 255.0) + 0.5);

            // Bottom (x, y + 1)
            idx = nextRowOffset + x * bpp;
            v = Unsafe.Add(ref pixelRef, idx) + e5;
            Unsafe.Add(ref pixelRef, idx) = (byte)(Math.Clamp(v, 0.0, 255.0) + 0.5);

            // Bottom-Right (x + 1, y + 1)
            idx = nextRowOffset + (x + 1) * bpp;
            v = Unsafe.Add(ref pixelRef, idx) + e1;
            Unsafe.Add(ref pixelRef, idx) = (byte)(Math.Clamp(v, 0.0, 255.0) + 0.5);
        }
        else
        {
            
            // Right
            if (x + 1 < width)
            {
                int idx = curRowOffset + (x + 1) * bpp;
                double v = Unsafe.Add(ref pixelRef, idx) + e7;
                Unsafe.Add(ref pixelRef, idx) = (byte)(Math.Clamp(v, 0.0, 255.0) + 0.5);
            }

            if (y + 1 < height)
            {
                // Bottom-Left
                if (x - 1 >= 0)
                {
                    int idx = nextRowOffset + (x - 1) * bpp;
                    double v = Unsafe.Add(ref pixelRef, idx) + e3;
                    Unsafe.Add(ref pixelRef, idx) = (byte)(Math.Clamp(v, 0.0, 255.0) + 0.5);
                }

                // Bottom
                {
                    int idx = nextRowOffset + x * bpp;
                    double v = Unsafe.Add(ref pixelRef, idx) + e5;
                    Unsafe.Add(ref pixelRef, idx) = (byte)(Math.Clamp(v, 0.0, 255.0) + 0.5);
                }

                // Bottom-Right
                if (x + 1 < width)
                {
                    int idx = nextRowOffset + (x + 1) * bpp;
                    double v = Unsafe.Add(ref pixelRef, idx) + e1;
                    Unsafe.Add(ref pixelRef, idx) = (byte)(Math.Clamp(v, 0.0, 255.0) + 0.5);
                }
            }
        }
    }
}