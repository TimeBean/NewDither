namespace Service.Dither.Infrastructure.Processor.Ordered.Constants;

/// <summary>
/// Provides standard halftone ordered dithering matrices.
/// </summary>
public class Halftone
{
    /// <summary>
    /// A 4x4 clustered-dot halftone matrix.
    /// Values increase from the center outward (distance-based ordering).
    /// </summary>
    public static readonly int[,] Halftone4 =
    {
        { 12, 4, 5, 13 },
        { 6, 0, 1, 7 },
        { 8, 2, 3, 9 },
        { 14, 10, 11, 15 }
    };

    /// <summary>
    /// An 8x8 clustered-dot halftone matrix.
    /// Values increase from the center outward (distance-based ordering).
    /// </summary>
    public static readonly int[,] Halftone8 =
    {
        { 60, 52, 44, 32, 33, 45, 53, 61 },
        { 54, 40, 24, 16, 17, 25, 41, 55 },
        { 46, 26, 12, 4, 5, 13, 27, 47 },
        { 34, 18, 6, 0, 1, 7, 19, 35 },
        { 36, 20, 8, 2, 3, 9, 21, 37 },
        { 48, 28, 14, 10, 11, 15, 29, 49 },
        { 56, 42, 30, 22, 23, 31, 43, 57 },
        { 62, 58, 50, 38, 39, 51, 59, 63 }
    };
}