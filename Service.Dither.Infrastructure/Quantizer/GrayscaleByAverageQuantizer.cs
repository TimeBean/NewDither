using Service.Dither.Core.Model.Quantizer;

namespace Service.Dither.Infrastructure.Quantizer;

public class GrayscaleByAverageQuantizer : IQuantizer
{
    public float[] Quantize(float[] pixels)
    {
        var pixel = pixels.Average();
        pixels[0] = pixel;
        pixels[1] = pixel;
        pixels[2] = pixel;
        
        return pixels;
        
    }
}