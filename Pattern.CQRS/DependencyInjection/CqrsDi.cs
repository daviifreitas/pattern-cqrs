using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Pattern.CQRS.Abstractions.Behavior;
using Pattern.CQRS.Abstractions.Messaging;

namespace Pattern.CQRS.DependencyInjection;

public static class CqrsDi
{
    public static void ConfigureCqrsDependencies(this IServiceCollection serviceCollection)
    {
        serviceCollection.Decorate(typeof(ICommandHandler<>), typeof(ValidationDecorator.CommandBaseHandler<>));
        serviceCollection.Decorate(typeof(ICommandHandler<,>), typeof(ValidationDecorator.CommandHandler<,>));

        serviceCollection.Decorate(typeof(ICommandHandler<>), typeof(TrackingDecorator.CommandBaseHandler<>));
        serviceCollection.Decorate(typeof(ICommandHandler<,>), typeof(TrackingDecorator.CommandHandler<,>));
        serviceCollection.Decorate(typeof(IQueryHandler<,>), typeof(TrackingDecorator.QueryHandler<,>));

        serviceCollection.Scan(scan => scan.FromAssembliesOf(typeof(CqrsDi))
            .AddClasses(classes => classes.AssignableTo(typeof(IQueryHandler<,>)), publicOnly: false)
            .AsImplementedInterfaces()
            .WithScopedLifetime()
            .AddClasses(classes => classes.AssignableTo(typeof(ICommandHandler<>)), publicOnly: false)
            .AsImplementedInterfaces()
            .WithScopedLifetime()
            .AddClasses(classes => classes.AssignableTo(typeof(ICommandHandler<,>)), publicOnly: false)
            .AsImplementedInterfaces()
            .WithScopedLifetime()
        );

        serviceCollection.AddValidatorsFromAssembly(typeof(CqrsDi).Assembly, includeInternalTypes: true);
        
        

    }
}