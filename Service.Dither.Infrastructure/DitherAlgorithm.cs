namespace Service.Dither.Infrastructure;

public enum DitherAlgorithm
{
    QuantizeOnly,

    FloydSteinberg,
    OptimizedFloydSteinberg,
    Burkes,

    JarvisJudiceNinke,
    Sierra3,
    Stucki,

    Atkinson,

    OrderedBayer,
    OrderedCluster,
    OrderedHalftone
}