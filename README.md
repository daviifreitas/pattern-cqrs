# Pattern.CQRS

A lightweight CQRS (Command Query Responsibility Segregation) library for .NET 8 with built-in support for validation and tracking decorators.

## 📋 Overview

This library provides a clean and simple implementation of the CQRS pattern, featuring:

- **Commands** - For write operations that modify state
- **Queries** - For read operations that return data
- **Result Pattern** - Unified response handling with success/failure states
- **Validation Decorator** - Automatic FluentValidation integration
- **Tracking Decorator** - Built-in logging/tracking for commands and queries
- **Automatic DI Registration** - Using Scrutor for assembly scanning

## 📦 Dependencies

- .NET 8.0
- FluentValidation 12.1.1
- Microsoft.Extensions.DependencyInjection 8.0.1
- Scrutor 7.0.0

## 🚀 Getting Started

### Installation

Add the project reference to your solution or install from NuGet (if published).

### Configuration

Register the CQRS dependencies in your `Program.cs` or `Startup.cs`:

```csharp
services.ConfigureCqrsDependencies();
```

This will automatically:
- Register all command and query handlers
- Apply validation decorators
- Apply tracking decorators
- Register all FluentValidation validators

## 📖 Usage

### Defining a Command (without response)

```csharp
public record CreateUserCommand(string Name, string Email) : ICommand;
```

### Defining a Command (with response)

```csharp
public record CreateOrderCommand(string ProductId, int Quantity) : ICommand<OrderResponse>;

public class OrderResponse
{
    public Guid OrderId { get; set; }
    public DateTime CreatedAt { get; set; }
}
```

### Implementing a Command Handler

```csharp
public class CreateUserCommandHandler : ICommandHandler<CreateUserCommand>
{
    public async Task<Result> Handle(CreateUserCommand command, CancellationToken cancellation)
    {
        // Your business logic here
        return Result.Success();
    }
}

public class CreateOrderCommandHandler : ICommandHandler<CreateOrderCommand, OrderResponse>
{
    public async Task<Result<OrderResponse>> Handle(CreateOrderCommand command, CancellationToken cancellation)
    {
        var response = new OrderResponse 
        { 
            OrderId = Guid.NewGuid(), 
            CreatedAt = DateTime.UtcNow 
        };
        
        return Result<OrderResponse>.SuccessWithResponse(response);
    }
}
```

### Defining a Query

```csharp
public record GetUserByIdQuery(Guid UserId) : IQuery<UserResponse>;

public class UserResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
}
```

### Implementing a Query Handler

```csharp
public class GetUserByIdQueryHandler : IQueryHandler<GetUserByIdQuery, UserResponse>
{
    public async Task<Result<UserResponse>> Handle(GetUserByIdQuery query, CancellationToken cancellation)
    {
        var user = new UserResponse 
        { 
            Id = query.UserId, 
            Name = "John Doe", 
            Email = "john@example.com" 
        };
        
        return Result<UserResponse>.SuccessWithResponse(user);
    }
}
```

### Adding Validation

Create a validator using FluentValidation:

```csharp
public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100);
            
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();
    }
}
```

Validators are automatically discovered and applied through the `ValidationDecorator`.

### Using the Result Pattern

```csharp
// Handling command results
Result result = await handler.Handle(command, cancellationToken);

if (result.IsSuccess)
{
    // Handle success
}
else
{
    // Handle failure - error message in result.Message
}

// Handling results with response
Result<OrderResponse> orderResult = await handler.Handle(orderCommand, cancellationToken);

if (orderResult.IsSuccess)
{
    OrderResponse response = orderResult.Response;
    // Use the response
}
```

## 🏗️ Architecture

### Abstractions

| Interface | Description |
|-----------|-------------|
| `ICommand` | Marker interface for commands without response |
| `ICommand<T>` | Marker interface for commands with response |
| `ICommandHandler<TCommand>` | Handler for commands without response |
| `ICommandHandler<TCommand, TResponse>` | Handler for commands with response |
| `IQuery<TResponse>` | Marker interface for queries |
| `IQueryHandler<TQuery, TResponse>` | Handler for queries |

### Decorators

#### ValidationDecorator
Automatically validates commands using FluentValidation before executing the handler. If validation fails, returns a `Result.Failure` with the validation errors.

#### TrackingDecorator
Logs/tracks the execution of commands and queries, including:
- Start of processing
- Successful completion
- Completion with errors

## 📁 Project Structure

```
Pattern.CQRS/
├── Abstractions/
│   ├── Behavior/
│   │   ├── TrackingDecorator.cs
│   │   └── ValidatorDecorator.cs
│   └── Messaging/
│       ├── ICommand.cs
│       ├── ICommandHandler.cs
│       ├── IQuery.cs
│       ├── IQueryHandler.cs
│       └── Result.cs
└── DependencyInjection/
    └── CqrsDi.cs
```

## 🤝 Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## 📄 License

This project is open source. Please check the license file for more details.
