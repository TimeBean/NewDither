using MediatR;

namespace Service.Quote.Application.GetById;

public record GetByIdCommand(int Id) : IRequest<Domain.Model.Quote>;