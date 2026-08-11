using Service.Dither.Core.Model.Quantizer;

namespace Service.Dither.Infrastructure.Quantizer;

public class GrayscaleByAverageLuminanceQuantizer : IQuantizer
{
    public float[] Quantize(float[] pixels)
    {
        var luminance = 0.2126 * pixels[0] + 0.7152 * pixels[1] + 0.0722 * pixels[2];
        var pixel = (float)Math.Round(luminance);
        for (var i = 0; i < pixels.Length; i++)
        {
            pixels[i] = pixel;
        }
        
        return pixels;
    }
}