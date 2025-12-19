using GFC.BlazorServer.Auth;
using GFC.BlazorServer.Data;
using GFC.BlazorServer.Data.Entities;
using GFC.BlazorServer.Models;
using GFC.BlazorServer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GFC.BlazorServer.Controllers;

[ApiController]
[Route("api/controller-test")]
[Authorize(Policy = AppPolicies.RequireAdmin)]
public class ControllerTestController : ControllerBase
{
    private readonly GfcDbContext _dbContext;
    private readonly IMengqiControllerClient _mengqiClient;
    private readonly ControllerTestService _testService;
    private readonly ILogger<ControllerTestController> _logger;

    public ControllerTestController(
        GfcDbContext dbContext,
        IMengqiControllerClient mengqiClient,
        ControllerTestService testService,
        ILogger<ControllerTestController> logger)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _mengqiClient = mengqiClient ?? throw new ArgumentNullException(nameof(mengqiClient));
        _testService = testService ?? throw new ArgumentNullException(nameof(testService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpGet("{id}/run-info")]
    public async Task<ActionResult<ApiResult<ControllerRunInfoDto>>> GetRunInfo(int id, CancellationToken cancellationToken = default)
    {
        var controller = await _dbContext.Controllers
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

        if (controller == null)
        {
            return NotFound(new ApiResult<ControllerRunInfoDto> { Success = false, Message = "Controller not found" });
        }

        var (result, latency) = await _testService.ExecuteWithLoggingAsync(
            id,
            "GetRunInfo",
            () => _mengqiClient.GetRunInfoAsync(controller.IpAddress, controller.Port, cancellationToken),
            cancellationToken);

        return Ok(result);
    }

    [HttpPost("{id}/open-door/{doorIndex}")]
    public async Task<ActionResult<ApiResult>> OpenDoor(int id, int doorIndex, CancellationToken cancellationToken = default)
    {
        var controller = await _dbContext.Controllers
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

        if (controller == null)
        {
            return NotFound(new ApiResult { Success = false, Message = "Controller not found" });
        }

        if (doorIndex < 1 || doorIndex > 4)
        {
            return BadRequest(new ApiResult { Success = false, Message = "Door index must be between 1 and 4" });
        }

        var (result, latency) = await _testService.ExecuteWithLoggingAsync(
            id,
            $"OpenDoor-{doorIndex}",
            () => _mengqiClient.OpenDoorAsync(controller.IpAddress, controller.Port, doorIndex, cancellationToken),
            cancellationToken);

        return Ok(result);
    }

    [HttpPost("{id}/close-door/{doorIndex}")]
    public async Task<ActionResult<ApiResult>> CloseDoor(int id, int doorIndex, CancellationToken cancellationToken = default)
    {
        var controller = await _dbContext.Controllers
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

        if (controller == null)
        {
            return NotFound(new ApiResult { Success = false, Message = "Controller not found" });
        }

        if (doorIndex < 1 || doorIndex > 4)
        {
            return BadRequest(new ApiResult { Success = false, Message = "Door index must be between 1 and 4" });
        }

        var (result, latency) = await _testService.ExecuteWithLoggingAsync(
            id,
            $"CloseDoor-{doorIndex}",
            () => _mengqiClient.CloseDoorAsync(controller.IpAddress, controller.Port, doorIndex, cancellationToken),
            cancellationToken);

        return Ok(result);
    }

    [HttpPost("{id}/hold-door/{doorIndex}")]
    public async Task<ActionResult<ApiResult>> HoldDoor(int id, int doorIndex, CancellationToken cancellationToken = default)
    {
        var controller = await _dbContext.Controllers
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

        if (controller == null)
        {
            return NotFound(new ApiResult { Success = false, Message = "Controller not found" });
        }

        if (doorIndex < 1 || doorIndex > 4)
        {
            return BadRequest(new ApiResult { Success = false, Message = "Door index must be between 1 and 4" });
        }

        var (result, latency) = await _testService.ExecuteWithLoggingAsync(
            id,
            $"HoldDoor-{doorIndex}",
            () => _mengqiClient.HoldDoorAsync(controller.IpAddress, controller.Port, doorIndex, cancellationToken),
            cancellationToken);

        return Ok(result);
    }

    [HttpGet("{id}/swipes/preview")]
    public async Task<ActionResult<ApiResult<LastRecordPreviewDto>>> GetSwipesPreview(int id, CancellationToken cancellationToken = default)
    {
        var controller = await _dbContext.Controllers
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

        if (controller == null)
        {
            return NotFound(new ApiResult<LastRecordPreviewDto> { Success = false, Message = "Controller not found" });
        }

        var (result, latency) = await _testService.ExecuteWithLoggingAsync(
            id,
            "GetLastRecordPreview",
            () => _mengqiClient.GetLastRecordPreviewAsync(controller.IpAddress, controller.Port, cancellationToken),
            cancellationToken);

        return Ok(result);
    }

    [HttpGet("{id}/swipes/{recordIndex}")]
    public async Task<ActionResult<ApiResult<SwipeRecordDto>>> GetSingleSwipe(int id, uint recordIndex, CancellationToken cancellationToken = default)
    {
        var controller = await _dbContext.Controllers
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

        if (controller == null)
        {
            return NotFound(new ApiResult<SwipeRecordDto> { Success = false, Message = "Controller not found" });
        }

        var (result, latency) = await _testService.ExecuteWithLoggingAsync(
            id,
            $"GetSingleSwipe-{recordIndex}",
            () => _mengqiClient.GetSingleSwipeAsync(controller.IpAddress, controller.Port, recordIndex, cancellationToken),
            cancellationToken);

        return Ok(result);
    }

    [HttpGet("{id}/get-doors")]
    public async Task<ActionResult<ApiResult<DoorStatusDto[]>>> GetDoors(int id, CancellationToken cancellationToken = default)
    {
        var controller = await _dbContext.Controllers
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

        if (controller == null)
        {
            return NotFound(new ApiResult<DoorStatusDto[]> { Success = false, Message = "Controller not found" });
        }

        var (result, latency) = await _testService.ExecuteWithLoggingAsync(
            id,
            "GetAllDoors",
            () => _mengqiClient.GetAllDoorsAsync(controller.IpAddress, controller.Port, cancellationToken),
            cancellationToken);

        return Ok(result);
    }

    [HttpGet("{id}/get-time")]
    public async Task<ActionResult<ApiResult<DateTime>>> GetTime(int id, CancellationToken cancellationToken = default)
    {
        var controller = await _dbContext.Controllers
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

        if (controller == null)
        {
            return NotFound(new ApiResult<DateTime> { Success = false, Message = "Controller not found" });
        }

        var (result, latency) = await _testService.ExecuteWithLoggingAsync(
            id,
            "GetTime",
            () => _mengqiClient.GetTimeAsync(controller.IpAddress, controller.Port, cancellationToken),
            cancellationToken);

        return Ok(result);
    }

    [HttpPost("{id}/set-time")]
    public async Task<ActionResult<ApiResult>> SetTime(int id, [FromBody] SetTimeRequest request, CancellationToken cancellationToken = default)
    {
        var controller = await _dbContext.Controllers
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

        if (controller == null)
        {
            return NotFound(new ApiResult { Success = false, Message = "Controller not found" });
        }

        if (request == null || !request.Time.HasValue)
        {
            return BadRequest(new ApiResult { Success = false, Message = "Time is required" });
        }

        var (result, latency) = await _testService.ExecuteWithLoggingAsync(
            id,
            "SetTime",
            () => _mengqiClient.SetTimeAsync(controller.IpAddress, controller.Port, request.Time.Value, cancellationToken),
            cancellationToken);

        return Ok(result);
    }

    public class SetTimeRequest
    {
        public DateTime? Time { get; set; }
    }
}

