USE ClubMembership;
SELECT 'ControllerLastIndexes' as TableName, * FROM ControllerLastIndexes;
SELECT TOP 20 'RecentEvents' as Type, EventType, ReasonCode, CardNumber, DoorId, RawIndex, CreatedUtc FROM ControllerEvents ORDER BY CreatedUtc DESC;
