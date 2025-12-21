using GFC.BlazorServer.Data.Entities;

namespace GFC.BlazorServer.Services.Controllers;

public class SimulationTraceCreateRequest
{
    public string Operation { get; set; } = string.Empty;
    public int? ControllerId { get; set; }
    public int? DoorId { get; set; }
    public long? CardNumber { get; set; }
    public int? MemberId { get; set; }
    public string? RequestSummary { get; set; }
    public string? RequestPayloadJson { get; set; }
    public string? RequestPayloadRaw { get; set; }
    public string? ExpectedResponseSummary { get; set; }
    public string? ExpectedResponsePayloadJson { get; set; }
    public string? TriggerPage { get; set; }
    public int? UserId { get; set; }
}

public interface ISimulationTraceService
{
    Task<long> CreateTraceAsync(SimulationTraceCreateRequest request, CancellationToken ct = default);

    Task UpdateResultAsync(long traceId, string resultStatus, string? resultDetails, CancellationToken ct = default);

    Task<(int ok, int partial, int failed, int pending)> GetStatusCountsAsync(
        string? operation = null,
        int? controllerId = null,
        int? doorId = null,
        long? cardNumber = null,
        int? memberId = null,
        CancellationToken ct = default);

    Task<IReadOnlyList<SimulationControllerTrace>> GetTracesAsync(
        string? operation = null,
        string? resultStatus = null,
        int? controllerId = null,
        int? doorId = null,
        long? cardNumber = null,
        int? memberId = null,
        int page = 1,
        int pageSize = 50,
        CancellationToken ct = default);

    Task<IReadOnlyList<SimulationControllerTrace>> GetTracesForExportAsync(
        string? operation = null,
        string? resultStatus = null,
        int? controllerId = null,
        int? doorId = null,
        long? cardNumber = null,
        int? memberId = null,
        CancellationToken ct = default);

    Task<IReadOnlyList<SimulationControllerTrace>> GetTracesForReplayAsync(
        string sessionFilter,
        CancellationToken ct = default);

    Task<SimulationControllerTrace?> GetTraceByIdAsync(long id, CancellationToken ct = default);
}
