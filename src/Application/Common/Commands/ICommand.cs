using MediatR;

namespace TestMillion.Application.Common.Commands;

public interface ICommand : IRequest
{
}

public interface ICommand<out TResult> : IRequest<TResult>
{
}