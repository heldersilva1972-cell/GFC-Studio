USE ClubMembership;
-- Clean slate: Remove all events and reset sync
DELETE FROM ControllerEvents;
DELETE FROM ControllerLastIndexes;
GO
