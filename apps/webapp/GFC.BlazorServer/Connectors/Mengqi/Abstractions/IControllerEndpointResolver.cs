using System;
using GFC.BlazorServer.Connectors.Mengqi.Configuration;

namespace GFC.BlazorServer.Connectors.Mengqi.Abstractions;

/// <summary>
///     Resolves controller serial numbers to concrete network endpoints (IP/port plus security metadata).
/// </summary>
public interface IControllerEndpointResolver
{
    /// <summary>
    ///     Returns the connection details for the given controller serial number.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when the serial number is unknown.</exception>
    ControllerEndpoint Resolve(uint controllerSn);
}


