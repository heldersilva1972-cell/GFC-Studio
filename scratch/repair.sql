WITH SheetGameCounts AS (
    SELECT SessionId, SheetColor, COUNT(DISTINCT GameName) as GameCount
    FROM BingoGameEntries
    GROUP BY SessionId, SheetColor
)
UPDATE ge
SET 
    ge.PrizePaid = ge.PrizePaid * sgc.GameCount,
    ge.NetProceeds = ge.GrossReceipts - ge.LotteryTake - (ge.PrizePaid * sgc.GameCount)
FROM BingoGameEntries ge
JOIN SheetGameCounts sgc ON ge.SessionId = sgc.SessionId AND ge.SheetColor = sgc.SheetColor
JOIN BingoSessions s ON ge.SessionId = s.Id
WHERE sgc.GameCount > 1
  AND s.SessionDate >= '2026-05-01' 
  AND s.SessionDate < '2026-06-01';

UPDATE s
SET 
    s.TotalPrizesPaid = ISNULL((SELECT SUM(PrizePaid) FROM BingoGameEntries WHERE SessionId = s.Id), 0),
    s.TotalClubTake = s.TotalGrossReceipts - ISNULL((SELECT SUM(PrizePaid) FROM BingoGameEntries WHERE SessionId = s.Id), 0) - s.TotalLotteryTake - s.RoundingAdjustment
FROM BingoSessions s
WHERE s.SessionDate >= '2026-05-01' 
  AND s.SessionDate < '2026-06-01';
