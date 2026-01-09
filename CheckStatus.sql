USE ClubMembership;
SELECT 'Status' as Type, ControllerId, LastRecordIndex FROM ControllerLastIndexes;
SELECT COUNT(*) as EventCount FROM ControllerEvents;
SELECT TOP 5 'NewEvent' as Type, CreatedUtc, TimestampUtc, RawIndex, CardNumber FROM ControllerEvents ORDER BY CreatedUtc DESC;
