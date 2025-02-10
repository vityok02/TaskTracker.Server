using Domain.Shared;
using MediatR;

namespace Application.Abstract.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}
