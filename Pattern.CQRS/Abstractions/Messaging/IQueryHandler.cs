namespace Pattern.CQRS.Abstractions.Messaging;

public interface IQueryHandler<in TQuery, TResponse> where TQuery : IQuery<TResponse> where TResponse : class
{
    Task<Result<TResponse>> Handle(TQuery query, CancellationToken cancellation);
}