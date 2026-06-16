namespace Service.Dither.Infrastructure.Processor.Ordered.Constants;

/// <summary>
/// Contains static threshold matrices (Bayer ordered dithering matrices) 
/// </summary>
public class Bayer
{
    /// <summary>
    /// A 2x2 Bayer matrix.
    /// Values range from 0 to 3.
    /// </summary>
    /// <value>
    /// Normalization formula: (M[i,j] + 1) / 4
    /// </value>
    public static readonly int[,] Bayer2X2 =
    {
        { 0, 2 },
        { 3, 1 }
    };

    /// <summary>
    /// A 4x4 Bayer matrix.
    /// Derived recursively from the 2x2 matrix. Values range from 0 to 15.
    /// </summary>
    /// <value>
    /// Normalization formula: (M[i,j] + 1) / 16
    /// </value>
    public static readonly int[,] Bayer4X4 =
    {
        { 0, 8, 2, 10 },
        { 12, 4, 14, 6 },
        { 3, 11, 1, 9 },
        { 15, 7, 13, 5 }
    };

    /// <summary>
    /// An 8x8 Bayer matrix.
    /// Used for generating smoother gradients. Values range from 0 to 63.
    /// </summary>
    /// <value>
    /// Normalization formula: (M[i,j] + 1) / 64
    /// </value>
    public static readonly int[,] Bayer8X8 =
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