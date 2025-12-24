-- [NEW]
-- GFC Performance Optimization Script
-- Phase 9: Stability, Performance Tuning & User Handover
-- Description: This script adds a non-clustered index to the ControllerEvents table to improve query performance.

-- Ensure this script is idempotent. It should be safe to run multiple times.

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_ControllerEvents_Timestamp_Desc' AND object_id = OBJECT_ID('dbo.ControllerEvents'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_ControllerEvents_Timestamp_Desc
    ON dbo.ControllerEvents (Timestamp DESC);
    PRINT 'Index IX_ControllerEvents_Timestamp_Desc created successfully.';
END
ELSE
BEGIN
    PRINT 'Index IX_ControllerEvents_Timestamp_Desc already exists.';
END
