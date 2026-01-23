USE ClubMembership;

-- 1. Delete events with invalid timestamps (Year 1 / Dec 31)
DELETE FROM ControllerEvents WHERE TimestampUtc < '2000-01-01';

-- 2. Reset Sync Pointer to force re-fetch
DELETE FROM ControllerLastIndexes;
INSERT INTO ControllerLastIndexes (ControllerId, LastRecordIndex) VALUES (2, 134284000);
GO
