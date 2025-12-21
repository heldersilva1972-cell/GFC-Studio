using System;

namespace Gfc.Agent.Api.Models;

public sealed class SyncTimeRequest
{
    public DateTime? ServerTimeUtc { get; set; }
}

