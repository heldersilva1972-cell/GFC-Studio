using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GFC.Core.Models.Finance;

namespace GFC.Core.Interfaces;

public interface ILotterySettlementService
{
    /// <summary>
    /// Gets all weekly settlements for a specific year, including calculated dues.
    /// </summary>
    Task<IEnumerable<LotteryWeeklySettlement>> GetWeeklySettlementsForYearAsync(int year);

    /// <summary>
    /// Records a manual payment (sweep) for a weekly settlement.
    /// </summary>
    Task SettleWeekManualAsync(int settlementId, string username, string referenceNumber, DateTime settleDate);

    /// <summary>
    /// Automatically generates unpaid bills in the system for completed weeks, if enabled in settings.
    /// </summary>
    Task SyncWeeklyBillsAsync();

    /// <summary>
    /// Recalculates and updates the weekly dues for a given year based on night shift closeouts.
    /// </summary>
    Task RecalculateWeeklyDuesAsync(int year);
}
