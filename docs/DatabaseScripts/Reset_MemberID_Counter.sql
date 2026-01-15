-- =============================================
-- Reset Member ID Auto-Increment Counter
-- Created: 2026-01-15
-- Purpose: Reset the identity seed to 6 after deleting members 6-200
-- =============================================

-- Check current identity value
DBCC CHECKIDENT ('Members', NORESEED);

-- Reset the identity counter to 5 (next insert will be 6)
DBCC CHECKIDENT ('Members', RESEED, 5);

-- Verify the new value
DBCC CHECKIDENT ('Members', NORESEED);

PRINT 'Identity counter reset. Next member will be ID 6.';
