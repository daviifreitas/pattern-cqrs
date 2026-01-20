using System.Text;
using FluentValidation;
using FluentValidation.Results;
using Pattern.CQRS.Abstractions.Messaging;

namespace Pattern.CQRS.Abstractions.Behavior;

internal static class ValidationDecorator
{
    internal sealed class CommandHandler<TCommand, TResponse>(
        ICommandHandler<TCommand, TResponse> innerHandler,
        IEnumerable<IValidator<TCommand>> validators)
        : ICommandHandler<TCommand, TResponse>
        where TCommand : ICommand<TResponse>
        where TResponse : class
    {
        public async Task<Result<TResponse>> Handle(TCommand command, CancellationToken cancellation)
        {
            ValidationFailure[] validationFailures = await ValidateAsync(command, validators);

            if (validationFailures.Length == 0) return await innerHandler.Handle(command, cancellation);

            string errorMessage = FormatErrorMessage(validationFailures);

            return Result<TResponse>.FailureWithResponse(default, errorMessage);
        }
    }

    internal sealed class CommandBaseHandler<TCommand>(
        ICommandHandler<TCommand> innerHandler,
        IEnumerable<IValidator<TCommand>> validators)
        : ICommandHandler<TCommand>
        where TCommand : ICommand
    {
        public async Task<Result> Handle(TCommand command, CancellationToken cancellation)
        {
            ValidationFailure[] validationFailures = await ValidateAsync(command, validators);

            if (validationFailures.Length == 0) return await innerHandler.Handle(command, cancellation);
            
            string errorMessage = FormatErrorMessage(validationFailures);
            
            return Result.Failure(errorMessage);
        }
    }

    private static async Task<ValidationFailure[]> ValidateAsync<TCommand>(
        TCommand command,
        IEnumerable<IValidator<TCommand>> validators)
    {
        IValidator<TCommand>[] enumerable = validators as IValidator<TCommand>[] ?? validators.ToArray();

        if (!enumerable.Any()) return [];

        ValidationContext<TCommand> context = new ValidationContext<TCommand>(command);

        FluentValidation.Results.ValidationResult[] validationResults = await Task.WhenAll(
            enumerable.Select(validator => validator.ValidateAsync(context)));

        ValidationFailure[] validationFailures = validationResults
            .Where(validationResult => !validationResult.IsValid)
            .SelectMany(validationResult => validationResult.Errors)
            .ToArray();

        return validationFailures;
    }

    private static string FormatErrorMessage(ValidationFailure[] validationFailures)
    {
        StringBuilder responseErrors = new();
        foreach (ValidationFailure validationFailure in validationFailures)
        {
            responseErrors.AppendLine(validationFailure.ErrorMessage);
        }

        return responseErrors.ToString();
    }
}