namespace GFC.BlazorServer.Models;

public class SystemDiagnosticsInfo
{
    public string EnvironmentName { get; set; } = string.Empty;
    public string ApplicationVersion { get; set; } = string.Empty;
    public string DatabaseProvider { get; set; } = string.Empty;
    public string DatabaseServer { get; set; } = string.Empty;
    public string DatabaseName { get; set; } = string.Empty;
    public string GfcConnectionStringName { get; set; } = "GFC";
    public string AgentApiBaseUrl { get; set; } = string.Empty;
    public bool? AgentApiReachable { get; set; }
    public string? AgentApiError { get; set; }
}

