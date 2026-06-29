namespace Service.WebSite.Domain.Service;

public interface IDitherService
{
    Task<byte[]?> ProcessAsync(Stream fileStream, string fileName, string contentType,
        DitherAlgorithm ditherAlgorithm, QuantizationAlgorithm quantizationAlgorithm);
}
