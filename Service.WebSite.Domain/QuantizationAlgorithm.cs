using System.ComponentModel.DataAnnotations;

namespace Service.WebSite.Domain;

public enum QuantizationAlgorithm
{
    [Display(Name = "Linear 2")]
    Linear2,

    [Display(Name = "Linear 4")]
    Linear4,

    [Display(Name = "Linear 8")]
    Linear8,

    [Display(Name = "Linear 16")]
    Linear16,

    [Display(Name = "Linear 32")]
    Linear32,

    [Display(Name = "Linear 64")]
    Linear64,
    
    [Display(Name = "Grayscale (Average)")]
    GrayscaleByAverage,
}