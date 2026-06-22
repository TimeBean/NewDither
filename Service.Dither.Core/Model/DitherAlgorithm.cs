namespace Service.Dither.Core.Model;

public enum DitherAlgorithm
{
    QuantizeOnly,


    Burkes,
    FloydSteinberg,

    JarvisJudiceNinke,
    Sierra3,
    Stucki,

    Atkinson,


    OrderedBayer2X2,
    OrderedBayer4X4,
    OrderedBayer8X8,

    OrderedCluster4X4,
    OrderedCluster8X8,

    OrderedHalftone4X4,
    OrderedHalftone8X8
}