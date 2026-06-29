using Service.Dither.Core.Exception;
using Service.Dither.Core.Model.Quantizer;

namespace Service.Dither.Infrastructure.Quantizer;

/// <summary>
/// Represents a linear quantizer for processing pixels.
/// </summary>
public class LinearQuantizer : IQuantizer
{
    /// <summary>
    /// Initializes a new instance of the <see cref="LinearQuantizer"/> class with the specified number of levels.
    /// </summary>
    /// <param name="levels">The number of quantization levels.</param>
    public LinearQuantizer(byte levels)
    {
        if (levels is <= 1 or >= byte.MaxValue)
        {
            throw new WrongQuantityException($"Level number must be between 2 and 255, inclusive: {Levels}");
        }
        
        Levels = levels;
    }

    /// <summary>
    /// Gets the specified number of quantization levels.
    /// </summary>
    private byte Levels { get; }

    /// <summary>
    /// Quantizes an array of pixel values, reducing the number of gradations to the specified <see cref="Levels"/>.
    /// </summary>
    /// <param name="pixels">An array of the original pixel values.</param>
    /// <returns>A new array containing the quantized (rounded to the nearest level) pixel values.</returns>
    /// <exception cref="WrongQuantityException">
    /// Thrown when the number of levels is less than or equal to 1, or greater than or equal to 256.
    /// </exception>
    public float[] Quantize(float[] pixels)
    {
        var levelsMinusOne = Levels - 1;
    
        var toIndexCoefficient = levelsMinusOne / 255.0;
        var toValueCoefficient = 255.0 / levelsMinusOne;

        for (var i = 0; i < pixels.Length; i++)
        {
            var index = (int)(pixels[i] * toIndexCoefficient + 0.5);
        
            if (index < 0) index = 0;
            else if (index > levelsMinusOne) index = levelsMinusOne;

            pixels[i] = (float)(index * toValueCoefficient);
        }

        return pixels;
    }
}