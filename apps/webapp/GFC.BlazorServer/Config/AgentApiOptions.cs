namespace GFC.BlazorServer.Configuration;

public sealed class AgentApiOptions
{
    public string BaseUrl { get; set; } = string.Empty;

    public string ApiKey { get; set; } = string.Empty;

    public double RequestTimeoutSeconds { get; set; } = 10;

    public List<ControllerConfig> Controllers { get; set; } = new();

    public class ControllerConfig
    {
        public uint SerialNumber { get; set; }
        public string IpAddress { get; set; } = string.Empty;
        public int UdpPort { get; set; } = 60000;
        public int TcpPort { get; set; } = 60000;
        public string? CommPassword { get; set; }
    }
}
