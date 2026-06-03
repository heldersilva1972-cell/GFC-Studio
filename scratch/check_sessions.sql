SELECT Id, SessionDate, AdmissionCount, TotalGrossReceipts, TotalPrizesPaid 
FROM BingoSessions 
WHERE SessionDate >= '2026-05-01' AND SessionDate < '2026-06-01'
ORDER BY SessionDate;
