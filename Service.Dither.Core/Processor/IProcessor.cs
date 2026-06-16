using Service.Dither.Core.Quantizer;

namespace Service.Dither.Core.Processor;

public interface IProcessor
{
    public int Width { get; }
    public int Height { get; }
    public int RowBytes { get; }
    public int BytesPerPixel { get; }

    public void Process(byte[] pixels, IQuantizer quantizer);
}