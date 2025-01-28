using Domain.Abstract;
using MediatR;

namespace Application.Abstract.Messaging;

public interface ICommand : IRequest<Result>
{
}

public interface ICommand<TResponse> : IRequest<Result<TResponse>>
{
}