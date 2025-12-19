namespace GFC.BlazorServer.Configuration;

public sealed class AgentApiOptions
{
    public string BaseUrl { get; set; } = string.Empty;

    public string ApiKey { get; set; } = string.Empty;

    public double RequestTimeoutSeconds { get; set; } = 10;
}
