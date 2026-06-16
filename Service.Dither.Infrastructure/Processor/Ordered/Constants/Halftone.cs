namespace Service.Dither.Infrastructure.Processor.Ordered.Constants;

/// <summary>
/// Provides standard halftone ordered dithering matrices.
/// </summary>
public class Halftone
{
    /// <summary>
    /// A 4x4 Halftone dither matrix.
    /// Values range from 0 to 15.
    /// </summary>
    /// <value>
    /// Normalization formula: (M[i,j] + 1) / 16
    /// </value>
    public static readonly int[,] Halftone4 =
    {
        { 0, 12, 3, 15 },
        { 8, 4, 11, 7 },
        { 2, 14, 1, 13 },
        { 10, 6, 9, 5 }
    };

    /// <summary>
    /// A 8x8 Halftone dither matrix.
    /// Values range from 0 to 63.
    /// </summary>
    /// <value>
    /// Normalization formula: (M[i,j] + 1) / 64
    /// </value>
    public static readonly int[,] Halftone8 =
    {
        { 0, 32, 8, 40, 2, 34, 10, 42 },
        { 48, 16, 56, 24, 50, 18, 58, 26 },
        { 12, 44, 4, 36, 14, 46, 6, 38 },
        { 60, 28, 52, 20, 62, 30, 54, 22 },
        { 3, 35, 11, 43, 1, 33, 9, 41 },
        { 51, 19, 59, 27, 49, 17, 57, 25 },
        { 15, 47, 7, 39, 13, 45, 5, 37 },
        { 63, 31, 55, 23, 61, 29, 53, 21 }
    };
}