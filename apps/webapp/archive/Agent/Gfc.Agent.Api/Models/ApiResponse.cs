namespace Gfc.Agent.Api.Models;

public record ApiResponse(bool Success, string Message)
{
    public static ApiResponse Ok(string message = "OK") => new(true, message);

    public static ApiResponse Failure(string message) => new(false, message);
}

public record ApiResponse<T>(bool Success, string Message, T? Data)
{
    public static ApiResponse<T> Ok(T data, string message = "OK") => new(true, message, data);

    public static ApiResponse<T> Failure(string message) => new(false, message, default);
}

