namespace GFC.BlazorServer.Models;

public sealed class ApiResult
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
}

public sealed class ApiResult<T>
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }
}

