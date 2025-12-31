DECLARE @ConstraintName nvarchar(200)
SELECT @ConstraintName = Name FROM sys.default_constraints
WHERE parent_object_id = OBJECT_ID('Controllers')
AND parent_column_id = (SELECT column_id FROM sys.columns WHERE object_id = OBJECT_ID('Controllers') AND name = 'DoorCount')

IF @ConstraintName IS NOT NULL
BEGIN
    EXEC('ALTER TABLE Controllers DROP CONSTRAINT ' + @ConstraintName)
END

IF EXISTS(SELECT 1 FROM sys.columns WHERE Name = N'DoorCount' AND Object_ID = Object_ID(N'Controllers'))
BEGIN
    ALTER TABLE Controllers DROP COLUMN DoorCount;
END
