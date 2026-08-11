using Service.Dither.Core.Model.Quantizer;

namespace Service.Dither.Infrastructure.Quantizer;

public class GrayscaleByAverageQuantizer : IQuantizer
{
    public float[] Quantize(float[] pixels)
    {
        var pixel = (float)Math.Round(pixels.Average());
        for (var i = 0; i < pixels.Length; i++)
        {
            pixels[i] = pixel;
        }
        
        return pixels;
    }
}