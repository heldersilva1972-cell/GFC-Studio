-- ==========================================================
-- GFC DOOR ACTIVITY FRESH START
-- ==========================================================
-- This script permanently deletes all door access events.
-- It also resets the sync counters so the system won't 
-- immediately re-download old history from the controllers.
-- ==========================================================

USE [ClubMembership];
GO

-- 1. Clear the door activity logs
TRUNCATE TABLE [dbo].[ControllerEvents];

-- 2. Reset the controller sync pointers
-- This ensures the system only grabs NEW events from this moment forward.
TRUNCATE TABLE [dbo].[ControllerLastIndexes];

PRINT 'Door Activity Logs and sync counters have been cleared. Starting fresh!';
GO
