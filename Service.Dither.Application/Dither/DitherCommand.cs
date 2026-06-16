using MediatR;
using Service.Dither.Core.Processor;
using Service.Dither.Core.Quantizer;

namespace Service.Dither.Application.Dither;

public record DitherCommand(byte[] Pixels, IProcessor Processor, IQuantizer Quantizer) : IRequest;