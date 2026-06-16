namespace Service.Dither.Infrastructure.Processor.Ordered.Constants;

/// <summary>
/// Provides predefined cluster ordered dithering matrices.
/// </summary>
public class Cluster
{
    /// <summary>
    /// A 4x4 cluster ordered dithering matrix.
    /// Values range from 1 to 16.
    /// </summary>
    /// <value>
    /// Normalization formula: M[i,j] / 16
    /// </value>
    public static readonly int[,] Cluster4X4 =
    {
        { 7, 13, 11, 4 },
        { 12, 16, 14, 8 },
        { 10, 15, 6, 2 },
        { 5, 9, 3, 1 }
    };

    /// <summary>
    /// An 8x8 cluster ordered dithering matrix.
    /// Values range from 1 to 50.
    /// </summary>
    /// <value>
    /// Normalization formula: M[i,j] / 50
    /// </value>
    public static readonly int[,] Cluster8X8 =
    {
        { 49, 41, 33, 25, 24, 32, 40, 48 },
        { 42, 34, 26, 18, 17, 25, 33, 41 },
        { 35, 27, 19, 11, 10, 18, 26, 34 },
        { 28, 20, 12, 4, 3, 11, 19, 27 },
        { 29, 21, 13, 5, 1, 9, 17, 25 },
        { 36, 28, 20, 12, 6, 14, 22, 30 },
        { 43, 35, 27, 19, 13, 21, 29, 37 },
        { 50, 42, 34, 26, 20, 28, 36, 44 }
    };
}