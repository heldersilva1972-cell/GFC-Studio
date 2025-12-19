using System;
using System.Collections.Generic;
using GFC.Core.Services;

namespace GFC.BlazorServer.Models;

public class MemberDoorAccessResponse
{
    public IReadOnlyList<MemberDoorAccessDto> Access { get; init; } = Array.Empty<MemberDoorAccessDto>();
    public KeyCardEligibilityResult Eligibility { get; init; } = new(false, false, false, false, false, null, string.Empty);
}
