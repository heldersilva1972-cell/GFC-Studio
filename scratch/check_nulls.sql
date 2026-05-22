USE [ClubMembership];
GO
SELECT Id FROM PosZReports 
WHERE Id IS NULL 
   OR Timestamp IS NULL 
   OR TerminalName IS NULL 
   OR BartenderName IS NULL 
   OR CashTotal IS NULL 
   OR InventoryPullsJson IS NULL 
   OR IsSynced IS NULL 
   OR TotalGrossSales IS NULL 
   OR SalesSummaryJson IS NULL 
   OR BanquetSummaryJson IS NULL 
   OR TokenCredits IS NULL;
GO
