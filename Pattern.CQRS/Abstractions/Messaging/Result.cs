namespace Pattern.CQRS.Abstractions.Messaging;

public class Result<TValue> : Result where TValue : class
{
    private Result(TValue? response, string erroMessage, bool isSuccess) : base(erroMessage, isSuccess)
    {
        Response = response;
    }

    public TValue? Response { get; set; }

    public static Result<TValue> FailureWithResponse(TValue response, string errorMessage) =>
        new(response, errorMessage, false);

    public static Result<TValue> SuccessWithResponse(TValue response) =>
        new(response, string.Empty, true);
}

public class Result
{
    protected Result(string erroMessage, bool isIsSuccess)
    {
        Message = erroMessage;
        IsSuccess = isIsSuccess;
    }

    public string Message { get; set; }
    public bool IsSuccess { get; set; }

    public static Result Failure(string errorMessage) =>
        new(errorMessage, false);

    public static Result Success() =>
        new(string.Empty, true);
}