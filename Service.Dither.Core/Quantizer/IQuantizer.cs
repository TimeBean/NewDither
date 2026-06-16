namespace Service.Dither.Core.Quantizer;

/// <summary>
/// Quantizes RGB color component values to a reduced set of discrete intensity levels.
/// </summary>
public interface IQuantizer
{
    /// <summary>
    /// Converts color component values to their quantized equivalents.
    /// </summary>
    /// <param name="pixels">
    /// A flat RGB component array containing values in the range of 0 to 255.
    /// Every three consecutive values represent the red, green, and blue components
    /// of a single pixel.
    /// </param>
    /// <returns>
    /// A new array containing the quantized RGB component values.
    /// The returned array has the same length and component ordering as the input array.
    /// </returns>
    float[] Quantize(float[] pixels);
}