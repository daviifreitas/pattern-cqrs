namespace Pattern.CQRS.Abstractions.Messaging;

public interface ICommandHandler<in TCommand> where TCommand : ICommand
{
    Task<Result> Handle(TCommand command, CancellationToken cancellation);
}

public interface ICommandHandler<in TCommand, TResponse>
    where TCommand : ICommand<TResponse>
    where TResponse: class
{
    Task<Result<TResponse>> Handle(TCommand command, CancellationToken cancellation);
}