using MediatR;

namespace Service.Quote.Application.GetRandom;

public record GetRandomCommand() : IRequest<Domain.Model.Quote>;