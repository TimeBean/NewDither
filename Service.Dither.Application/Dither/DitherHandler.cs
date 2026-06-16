using MediatR;

namespace Service.Dither.Application.Dither;

public class DitherHandler : IRequestHandler<DitherCommand>
{
    public Task Handle(DitherCommand request, CancellationToken cancellationToken)
    {
        request.Processor.Process(request.Pixels, request.Quantizer);
        
        return Task.CompletedTask;
    }
}