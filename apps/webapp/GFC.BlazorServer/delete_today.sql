DELETE FROM BarSaleEntries WHERE CAST(ISNULL(AdjustedSaleDate, SaleDate) AS DATE) = '2026-03-11';
DELETE FROM LotteryShifts WHERE CAST(ShiftDate AS DATE) = '2026-03-11';
GO
