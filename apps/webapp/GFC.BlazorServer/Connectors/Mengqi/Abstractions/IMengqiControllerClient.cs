using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GFC.BlazorServer.Connectors.Mengqi.Models;

namespace GFC.BlazorServer.Connectors.Mengqi.Abstractions;

/// <summary>
///     Provides the strongly-typed set of operations that the Agent can perform against a WG3000/Mengqi access controller.
///     The methods wrap the raw UDP/TCP protocol and expose a safe, async-friendly surface without leaking low-level details.
/// </summary>
public interface IMengqiControllerClient
{
    /// <summary>
    ///     Issues a remote door open command for the specified door number. Optionally overrides the relay pulse duration.
    /// </summary>
    Task OpenDoorAsync(uint controllerSn, int doorNo, int? durationSeconds = null, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Syncs the controller's RTC with the provided server time.
    /// </summary>
    Task SyncTimeAsync(uint controllerSn, DateTime localTime, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Adds a new card privilege or updates an existing one for the specified controller.
    /// </summary>
    Task AddOrUpdateCardAsync(uint controllerSn, CardPrivilegeModel model, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Deletes a single card (by card number) from the controller's privilege table.
    /// </summary>
    Task DeleteCardAsync(uint controllerSn, long cardNumber, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Uploads the provided set of card privileges in bulk using the controller's streaming/FLASH mechanisms.
    /// </summary>
    Task BulkUploadCardsAsync(uint controllerSn, IEnumerable<CardPrivilegeModel> cards, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Clears every card/privilege entry from the controller.
    /// </summary>
    Task ClearAllCardsAsync(uint controllerSn, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Reads the swipe/event buffer for all events after the provided last index.
    ///     Returns the ordered collection of events plus the last index that the controller reported.
    /// </summary>
    Task<(IReadOnlyList<ControllerEvent> Events, uint ControllerLastIndex)> GetNewEventsAsync(uint controllerSn, uint lastKnownIndex, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Sends an acknowledgement to the controller indicating which event index has been successfully processed.
    ///     This allows the controller to overwrite old events in its buffer.
    /// </summary>
    Task AcknowledgeEventsAsync(uint controllerSn, uint eventsReadIndex, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Retrieves the current run/status block (doors, relays, sensors) from the controller.
    /// </summary>
    Task<RunStatusModel> GetRunStatusAsync(uint controllerSn, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Reads a raw FLASH block from the specified area using the controller's SSI_FLASH commands.
    /// </summary>
    Task<byte[]> ReadFlashBlockAsync(uint controllerSn, FlashArea area, int start, int length, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Writes a raw FLASH block to the specified area using the controller's SSI_FLASH commands.
    /// </summary>
    Task WriteFlashBlockAsync(uint controllerSn, FlashArea area, int start, ReadOnlyMemory<byte> data, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Reads all controller time schedules (time zones + holidays + tasks) and maps them into a DTO.
    /// </summary>
    Task<TimeScheduleDto> ReadTimeSchedulesAsync(uint controllerSn, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Writes controller time schedules/holidays/tasks compiled by the application.
    /// </summary>
    Task WriteTimeSchedulesAsync(uint controllerSn, TimeScheduleWriteDto dto, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Reads the extended configuration structure from controller memory.
    /// </summary>
    Task<ExtendedConfigDto> ReadExtendedConfigAsync(uint controllerSn, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Writes extended per-door configuration (advanced door modes, relays, etc.).
    /// </summary>
    Task WriteExtendedDoorConfigAsync(uint controllerSn, ExtendedDoorConfigWriteDto dto, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Retrieves the controller's current network/IP configuration block.
    /// </summary>
    Task<ControllerNetworkConfig> GetNetworkConfigAsync(uint controllerSn, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Persists a new network/IP configuration block to the controller.
    /// </summary>
    Task SetNetworkConfigAsync(uint controllerSn, ControllerNetworkConfig config, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Updates the allowed PC IP list and communication password stored on the controller.
    /// </summary>
    Task SetAllowedPcAndCommPasswordAsync(uint controllerSn, AllowedPcConfig config, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Issues a safe reboot/reset command to the controller.
    /// </summary>
    Task RebootControllerAsync(uint controllerSn, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Broadcasts a search packet to discover all controllers on the subnet.
    /// </summary>
    Task<IEnumerable<DiscoveryResult>> DiscoverControllersAsync(CancellationToken cancellationToken = default);

    /// <summary>
    ///     Configures key door parameters (Control Mode, Relay Delay, Sensors, Interlock).
    /// </summary>
    Task SetDoorConfigAsync(uint controllerSn, int doorIndex, byte controlMode, byte relayDelay, byte doorSensor, byte interlock, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Retrieves detailed hardware info (IP, MAC, Firmware, Door Modes) from a specific controller.
    /// </summary>
    Task<DiscoveryResult?> GetHardwareInfoAsync(uint controllerSn, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Performs a 'Deep Reset' by clearing all privileges (0x54), synchronizing time (0x30),
    ///     and initializing doors based on database configuration or door count.
    /// </summary>
    /// <param name="controllerSn">Controller serial number</param>
    /// <param name="doorCount">Number of active doors (1-4). Unused doors will be disabled.</param>
    /// <param name="doorConfigs">Optional list of specific door settings to apply. If null, defaults are used.</param>
    /// <param name="primaryCard">Optional card to re-add immediately after reset (Step 5).</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task ResetControllerAsync(uint controllerSn, int doorCount = 4, IEnumerable<DoorHardwareConfig>? doorConfigs = null, CardPrivilegeModel? primaryCard = null, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Sends a raw byte array directly to the controller and returns the response.
    ///     WARNING: This bypasses protocol safety and encryption. Use only for low-level diagnostics.
    /// </summary>
    Task<byte[]> SendRawAsync(uint controllerSn, byte[] rawPacket, CancellationToken ct = default);
}


