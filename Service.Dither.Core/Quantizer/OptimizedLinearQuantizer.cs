using System.Runtime.CompilerServices;
using Service.Dither.Core.Exception;

namespace Service.Dither.Core.Quantizer;

/// <summary>
/// Provides an optimized linear quantization implementation using a Look-Up Table (LUT).
/// Supports parallel processing for large datasets and unsafe pointer operations for maximum performance.
/// </summary>
public class OptimizedLinearQuantizer : IQuantizer
{
    /// <summary>
    /// Initializes a new instance of the <see cref="OptimizedLinearQuantizer"/> class.
    /// </summary>
    /// <param name="levels">The number of quantization levels (must be between 2 and 255).</param>
    public OptimizedLinearQuantizer(int levels)
    {
        Levels = levels;
    }

    /// <summary>
    /// Gets the number of quantization levels.
    /// </summary>
    private int Levels { get; }

    /// <summary>
    /// The pixel count threshold above which execution is split into multiple parallel tasks.
    /// </summary>
    private const int ParallelThreshold = 1 << 16; // 65536

    /// <summary>
    /// Quantizes an array of pixel values based on the configured number of levels.
    /// </summary>
    /// <param name="pixels">The input array of pixel values (expected range is typically 0 to 255).</param>
    /// <returns>A new array containing the quantized pixel values.</returns>
    /// <exception cref="WrongLevelQuantityException">Thrown when the level count is less than 2 or greater than 255.</exception>
    public float[] Quantize(float[] pixels)
    {
        if (Levels is <= 1 or >= 256)
        {
            throw new WrongLevelQuantityException(
                $"Level number must be between 2 and 255, inclusive: {Levels}");
        }

        var lut = BuildLut();
        var result = new float[pixels.Length];

        if (pixels.Length < ParallelThreshold)
        {
            ApplyLut(pixels, result, lut, 0, pixels.Length);
        }
        else
        {
            Parallel.For(0, Environment.ProcessorCount, p =>
            {
                var chunk = pixels.Length / Environment.ProcessorCount;
                var start = p * chunk;
                
                var end = p == Environment.ProcessorCount - 1
                    ? pixels.Length
                    : start + chunk;

                ApplyLut(pixels, result, lut, start, end);
            });
        }

        return result;
    }

    /// <summary>
    /// Precomputes a 256-element Look-Up Table (LUT) mapping original intensities to quantized values.
    /// </summary>
    /// <returns>A precomputed float array representing the LUT.</returns>
    private float[] BuildLut()
    {
        var levelsMinusOne = Levels - 1;
        var toIndex = levelsMinusOne / 255.0;
        var toColor = 255.0 / levelsMinusOne;

        var lut = new float[256];

        for (var v = 0; v < 256; v++)
        {
            // Map the current 0-255 value to the corresponding quantization index
            var index = (int)Math.Round(v * toIndex);
            index = Math.Clamp(index, 0, levelsMinusOne);
            
            // Map the index back to the target color/intensity scale
            lut[v] = (float)(index * toColor);
        }

        return lut;
    }

    /// <summary>
    /// Core processing method that applies the LUT to a segment of the pixel array.
    /// Uses aggressive optimization and unsafe pointers to bypass bounds checking for speed.
    /// </summary>
    /// <param name="pixels">The source pixel array.</param>
    /// <param name="result">The destination pixel array.</param>
    /// <param name="lut">The Look-Up Table array.</param>
    /// <param name="start">The starting index of the segment (inclusive).</param>
    /// <param name="end">The ending index of the segment (exclusive).</param>
    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    private static unsafe void ApplyLut(
        float[] pixels, float[] result, float[] lut, int start, int end)
    {
        fixed (float* pSrc = pixels)
        fixed (float* pDst = result)
        fixed (float* pLut = lut)
        {
            for (var i = start; i < end; i++)
            {
                var v = pSrc[i];
                var idx = (int)(v + 0.5f);
                
                if (idx < 0) idx = 0;
                else if (idx > 255) idx = 255;

                pDst[i] = pLut[idx];
            }
        }
    }
}