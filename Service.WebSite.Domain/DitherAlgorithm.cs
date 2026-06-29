using System.ComponentModel.DataAnnotations;

namespace Service.WebSite.Domain;

public enum DitherAlgorithm
{
    [Display(Name = "Без дизеринга (Quantize Only)")]
    QuantizeOnly,

    [Display(Name = "Burkes", GroupName = "Error Diffusion")]
    Burkes,

    [Display(Name = "Floyd-Steinberg", GroupName = "Error Diffusion")]
    FloydSteinberg,

    [Display(Name = "Jarvis-Judice-Ninke", GroupName = "Error Diffusion")]
    JarvisJudiceNinke,

    [Display(Name = "Sierra 3", GroupName = "Error Diffusion")]
    Sierra3,

    [Display(Name = "Stucki", GroupName = "Error Diffusion")]
    Stucki,

    [Display(Name = "Atkinson", GroupName = "Error Diffusion")]
    Atkinson,

    [Display(Name = "Bayer 2×2", GroupName = "Ordered Bayer")]
    OrderedBayer2X2,

    [Display(Name = "Bayer 4×4", GroupName = "Ordered Bayer")]
    OrderedBayer4X4,

    [Display(Name = "Bayer 8×8", GroupName = "Ordered Bayer")]
    OrderedBayer8X8,

    [Display(Name = "Cluster 4×4", GroupName = "Ordered Cluster")]
    OrderedCluster4X4,

    [Display(Name = "Cluster 8×8", GroupName = "Ordered Cluster")]
    OrderedCluster8X8,

    [Display(Name = "Halftone 4×4", GroupName = "Ordered Halftone")]
    OrderedHalftone4X4,

    [Display(Name = "Halftone 8×8", GroupName = "Ordered Halftone")]
    OrderedHalftone8X8
}