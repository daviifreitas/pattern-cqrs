using Pattern.CQRS.Abstractions.Messaging;
using static System.Console;

namespace Pattern.CQRS.Abstractions.Behavior;

internal static class TrackingDecorator
{
    internal sealed class CommandHandler<TCommand, TResponse>(
        ICommandHandler<TCommand, TResponse> innerHandler)
        : ICommandHandler<TCommand, TResponse>
        where TCommand : ICommand<TResponse> where TResponse : class
    {
        public async Task<Result<TResponse>> Handle(TCommand command, CancellationToken cancellation)
        {
            string commandName = typeof(TCommand).Name;

            // logger.LogInformation("Processing command {Command}", commandName);
            WriteLine("Processing command {Command}", commandName);
            

            Result<TResponse> result = await innerHandler.Handle(command, cancellation);

            if (result.IsSuccess)
            {
                // logger.LogInformation("Completed command {Command}", commandName);
                WriteLine("Completed command {Command}", commandName);
            }
            else
            {
                // using (LogContext.PushProperty("Error", result.Error, true))
                // {
                //     logger.LogError("Completed command {Command} with error", commandName);
                // }
                WriteLine("Completed command {Command} with error", commandName);
            }

            return result;
        }
    }

    internal sealed class CommandBaseHandler<TCommand>(
        ICommandHandler<TCommand> innerHandler)
        : ICommandHandler<TCommand>
        where TCommand : ICommand
    {
        public async Task<Result> Handle(TCommand command, CancellationToken cancellation)
        {
            string commandName = typeof(TCommand).Name;

            // logger.LogInformation("Processing command {Command}", commandName);
            WriteLine("Processing command {Command}", commandName);

            Result result = await innerHandler.Handle(command, cancellation);

            if (result.IsSuccess)
            {
                // logger.LogInformation("Completed command {Command}", commandName);
                WriteLine("Completed command {Command}", commandName);
            }
            else
            {
                // using (LogContext.PushProperty("Error", result.Error, true))
                // {
                //     logger.LogError("Completed command {Command} with error", commandName);
                // }
                WriteLine("Completed command {Command} with error", commandName);
            }

            return result;
        }
    }

    internal sealed class QueryHandler<TQuery, TResponse>(
        IQueryHandler<TQuery, TResponse> innerHandler)
        : IQueryHandler<TQuery, TResponse>
        where TQuery : IQuery<TResponse> where TResponse : class
    {
        public async Task<Result<TResponse>> Handle(TQuery query, CancellationToken cancellation)
        {
            string queryName = typeof(TQuery).Name;

            // logger.LogInformation("Processing query {Query}", queryName);
            WriteLine("Processing query {Query}", queryName);

            Result<TResponse> result = await innerHandler.Handle(query, cancellation);

            if (result.IsSuccess)
            {
                // logger.LogInformation("Completed query {Query}", queryName);
                WriteLine("Completed query {Query}", queryName);
            }
            else
            {
                // using (LogContext.PushProperty("Error", result.Error, true))
                // {
                //     logger.LogError("Completed query {Query} with error", queryName);
                // }
                WriteLine("Completed query {Query} with error", queryName);
            }

            return result;
        }
    }
}
