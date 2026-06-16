using Service.Dither.Core.Exception;

namespace Service.Dither.Core.Quantizer;

/// <summary>
/// Represents a linear quantizer for processing pixels.
/// </summary>
[Obsolete("Use OptimizedLinearQuantizer instead", true)]
public class LinearQuantizer : IQuantizer
{
    /// <summary>
    /// Initializes a new instance of the <see cref="LinearQuantizer"/> class with the specified number of levels.
    /// </summary>
    /// <param name="levels">The number of quantization levels.</param>
    public LinearQuantizer(int levels)
    {
        Levels = levels;
    }

    /// <summary>
    /// Gets the specified number of quantization levels.
    /// </summary>
    private int Levels { get; }

    /// <summary>
    /// Quantizes an array of pixel values, reducing the number of gradations to the specified <see cref="Levels"/>.
    /// </summary>
    /// <param name="pixels">An array of the original pixel values.</param>
    /// <returns>A new array containing the quantized (rounded to the nearest level) pixel values.</returns>
    /// <exception cref="WrongLevelQuantityException">
    /// Thrown when the number of levels is less than or equal to 1, or greater than or equal to 256.
    /// </exception>
    public float[] Quantize(float[] pixels)
    {
        if (Levels is <= 1 or >= 256)
        {
            throw new WrongLevelQuantityException($"Level number must be between 2 and 255, inclusive: {Levels}");
        }
        
        var newPixels = new List<float>();
        foreach (var color in pixels)
        {
            var levelsMinusOne = Levels - 1;
            var index = (int)Math.Round(color * (levelsMinusOne / 255.0));
            if (index < 0) index = 0;
            if (index > levelsMinusOne) index = levelsMinusOne;

            var value = index * (255.0 / levelsMinusOne);

            newPixels.Add((float)value);
        }

        return newPixels.ToArray();
    }
}