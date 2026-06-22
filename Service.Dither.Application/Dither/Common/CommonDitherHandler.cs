using MediatR;

namespace Service.Dither.Application.Dither.Common;

public class CommonDitherHandler : IRequestHandler<CommonDitherCommand>
{
    public Task Handle(CommonDitherCommand request, CancellationToken cancellationToken)
    {
        request.Processor.Process(request.Pixels, request.Quantizer);
        
        return Task.CompletedTask;
    }
}