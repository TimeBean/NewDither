using Service.WebSite.Domain;

namespace Service.WebSite.Presentation.Mvc;

public class DitherProcessRequest
{
    public required IFormFile File { get; set; }
    public DitherAlgorithm DitherAlgorithm { get; set; }
    public QuantizationAlgorithm QuantizationAlgorithm { get; set; }
}