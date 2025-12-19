using GFC.BlazorServer.Models;

namespace GFC.BlazorServer.Services;

/// <summary>
/// Safe, read-only and test operations for Mengqi controllers.
/// NO destructive operations (no flash writes, no privilege clears, no bulk uploads).
/// </summary>
public interface IMengqiControllerClient
{
    /// <summary>
    /// Test connectivity to the controller.
    /// </summary>
    Task<ApiResult<bool>> PingAsync(string ipAddress, int port, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get run status information from the controller.
    /// </summary>
    Task<ApiResult<ControllerRunInfoDto>> GetRunInfoAsync(string ipAddress, int port, CancellationToken cancellationToken = default);

    /// <summary>
    /// Read the controller's current time.
    /// </summary>
    Task<ApiResult<DateTime>> GetTimeAsync(string ipAddress, int port, CancellationToken cancellationToken = default);

    /// <summary>
    /// Set the controller's time to the specified value.
    /// </summary>
    Task<ApiResult> SetTimeAsync(string ipAddress, int port, DateTime time, CancellationToken cancellationToken = default);

    /// <summary>
    /// Open a door (momentary unlock).
    /// </summary>
    Task<ApiResult> OpenDoorAsync(string ipAddress, int port, int doorIndex, CancellationToken cancellationToken = default);

    /// <summary>
    /// Close a door (lock).
    /// </summary>
    Task<ApiResult> CloseDoorAsync(string ipAddress, int port, int doorIndex, CancellationToken cancellationToken = default);

    /// <summary>
    /// Hold a door open (continuous unlock).
    /// </summary>
    Task<ApiResult> HoldDoorAsync(string ipAddress, int port, int doorIndex, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get a preview of the last few swipe records (without reading the full record).
    /// </summary>
    Task<ApiResult<LastRecordPreviewDto>> GetLastRecordPreviewAsync(string ipAddress, int port, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get a single swipe record by index.
    /// </summary>
    Task<ApiResult<SwipeRecordDto>> GetSingleSwipeAsync(string ipAddress, int port, uint recordIndex, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get all door status information.
    /// </summary>
    Task<ApiResult<DoorStatusDto[]>> GetAllDoorsAsync(string ipAddress, int port, CancellationToken cancellationToken = default);
}

