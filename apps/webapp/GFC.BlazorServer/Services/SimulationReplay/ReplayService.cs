using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GFC.BlazorServer.Services.Controllers;

namespace GFC.BlazorServer.Services.SimulationReplay;

public class ReplayService : IReplayService
{
    private readonly ISimulationTraceService _traceService;

    public ReplayService(ISimulationTraceService traceService)
    {
        _traceService = traceService;
    }

    public async Task<IReadOnlyList<ReplayStep>> BuildReplayStepsAsync(
        string sessionFilter,
        CancellationToken ct = default)
    {
        var traces = await _traceService.GetTracesForReplayAsync(sessionFilter, ct);

        return traces
            .OrderBy(t => t.TimestampUtc)
            .Select(t => new ReplayStep
            {
                Id = (int)t.Id,
                TimestampUtc = t.TimestampUtc,
                Operation = t.Operation,
                Summary = t.RequestSummary,
                OriginPage = t.TriggerPage,
                ResultStatus = t.ResultStatus,
                RequestSummary = t.RequestSummary,
                RequestJson = t.RequestPayloadJson,
                ExpectedResponseJson = t.ExpectedResponsePayloadJson,
                ActualResultJson = t.ResultDetails,
                ControllerId = t.ControllerId,
                DoorId = t.DoorId,
                CardNumber = t.CardNumber,
                MemberId = t.MemberId,
                HasWarning = t.ResultStatus == "Failed" || t.ResultStatus == "Partial",
                WarningMessage = t.ResultDetails
            })
            .ToList();
    }
}
