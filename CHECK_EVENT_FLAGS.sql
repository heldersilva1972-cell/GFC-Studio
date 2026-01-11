-- Check the most recent events and their flags
SELECT TOP 10
    Id,
    DoorOrReader,
    EventType,
    IsByCard,
    IsByButton,
    CardNumber,
    TimestampUtc,
    RawData
FROM ControllerEvents
ORDER BY Id DESC;
