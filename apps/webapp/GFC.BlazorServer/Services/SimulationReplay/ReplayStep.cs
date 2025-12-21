using System;

namespace GFC.BlazorServer.Services.SimulationReplay;

public class ReplayStep
{
    public int Id { get; set; }
    public DateTime TimestampUtc { get; set; }
    public string Operation { get; set; }
    public string Summary { get; set; }
    public string OriginPage { get; set; }
    public string ResultStatus { get; set; }

    public string RequestSummary { get; set; }
    public string RequestJson { get; set; }
    public string ExpectedResponseJson { get; set; }
    public string ActualResultJson { get; set; }

    public int? ControllerId { get; set; }
    public int? DoorId { get; set; }
    public long? CardNumber { get; set; }
    public int? MemberId { get; set; }

    public bool HasWarning { get; set; }
    public string WarningMessage { get; set; }
}
