-- ==========================================================
-- GFC AUDIT LOG FRESH START
-- ==========================================================
-- This script permanently deletes all entries from the AuditLogs table.
-- Use this if you want to clear your activity history and start fresh.
-- ==========================================================

USE [ClubMembership];
GO

-- Option 1: Fast delete (Truncate)
-- This is the most efficient way to clear a table completely.
TRUNCATE TABLE [dbo].[AuditLogs];

PRINT 'Audit logs have been successfully cleared. You are starting with a fresh history.';
GO
