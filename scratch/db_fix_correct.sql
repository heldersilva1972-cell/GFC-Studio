USE ClubMembership;

-- 1. Update fixed payout entries (PayoutMode = 0 and IsProgressive = 0)
WITH GameGross AS (
    SELECT SessionId, SheetColor, GameName, SUM(GrossReceipts) AS TotalGameGross
    FROM BingoGameEntries
    GROUP BY SessionId, SheetColor, GameName
)
UPDATE ge
SET 
    ge.PrizePaid = gd.DefaultPayout * (CASE WHEN gg.TotalGameGross > 0 THEN (ge.GrossReceipts / gg.TotalGameGross) ELSE 1.0 END),
    ge.NetProceeds = ge.GrossReceipts - ge.LotteryTake - (gd.DefaultPayout * (CASE WHEN gg.TotalGameGross > 0 THEN (ge.GrossReceipts / gg.TotalGameGross) ELSE 1.0 END))
FROM BingoGameEntries ge
JOIN BingoSessions s ON ge.SessionId = s.Id
JOIN BingoSheetDefinitions sd ON ge.SheetColor = sd.ColorName
JOIN BingoGameDefinitions gd ON gd.SheetDefinitionId = sd.Id AND gd.GameName = ge.GameName
JOIN GameGross gg ON ge.SessionId = gg.SessionId AND ge.SheetColor = gg.SheetColor AND ge.GameName = gg.GameName
WHERE gd.PayoutMode = 0 
  AND gd.IsProgressive = 0
  AND s.SessionDate >= '2026-05-01' 
  AND s.SessionDate < '2026-07-01';

-- 2. Update progressive entries
UPDATE ge
SET 
    ge.PrizePaid = 500.00,
    ge.NetProceeds = ge.GrossReceipts - ge.LotteryTake - 500.00
FROM BingoGameEntries ge
JOIN BingoSessions s ON ge.SessionId = s.Id
WHERE ge.GameName = 'progressive blackout'
  AND s.SessionDate >= '2026-05-01' 
  AND s.SessionDate < '2026-07-01';

-- 3. Recalculate session-level summaries
UPDATE s
SET 
    s.TotalPrizesPaid = ISNULL((SELECT SUM(PrizePaid) FROM BingoGameEntries WHERE SessionId = s.Id), 0),
    s.TotalClubTake = s.TotalGrossReceipts - ISNULL((SELECT SUM(PrizePaid) FROM BingoGameEntries WHERE SessionId = s.Id), 0) - s.TotalLotteryTake - s.RoundingAdjustment
FROM BingoSessions s
WHERE s.SessionDate >= '2026-05-01' 
  AND s.SessionDate < '2026-07-01';
