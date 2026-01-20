namespace Pattern.CQRS.Abstractions.Messaging;
public interface IQuery<TResponse> where TResponse : class;