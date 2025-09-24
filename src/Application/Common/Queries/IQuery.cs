using MediatR;

namespace TestMillion.Application.Common.Queries;

public interface IQuery<out TResult> : IRequest<TResult>
{
}