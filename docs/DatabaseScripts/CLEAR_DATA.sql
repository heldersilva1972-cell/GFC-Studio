-- =============================================
-- CLEAR DATA SCRIPT
-- Removes all data from the database tables while keeping the structure intact.
-- USAGE: Run this to return to a "Clean Slate" state without dropping tables.
-- =============================================

USE [ClubMembership];
GO

PRINT 'Clearing All Data...';

-- Disable Constraints
EXEC sp_MSforeachtable "ALTER TABLE ? NOCHECK CONSTRAINT all";

-- Delete Data (Order matters less with constraints disabled, but good practice)
DELETE FROM LoginHistory;
DELETE FROM DuesWaiverPeriods;
DELETE FROM GlobalNotes;
DELETE FROM PhysicalKeys;
DELETE FROM MemberKeycardAssignments;
DELETE FROM KeyCards;
DELETE FROM BarSaleEntries;
DELETE FROM StaffShifts;
DELETE FROM StaffMembers;
DELETE FROM NPQueueEntries;
DELETE FROM DuesPayments;
DELETE FROM AppUsers WHERE Username != 'admin'; -- Keep Admin!
DELETE FROM BoardAssignments;
DELETE FROM Members;

-- Re-enable Constraints
EXEC sp_MSforeachtable "ALTER TABLE ? WITH CHECK CHECK CONSTRAINT all";

PRINT '✓ All data cleared (Admin user preserved).';
