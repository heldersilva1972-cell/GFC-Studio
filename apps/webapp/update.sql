DECLARE @InputString VARCHAR(100) = '851181';
DECLARE @Hash VARBINARY(32) = HASHBYTES('SHA2_256', @InputString);
DECLARE @Base64Hash VARCHAR(100) = CAST('' AS XML).value('xs:base64Binary(sql:variable("@Hash"))', 'VARCHAR(100)');

UPDATE Users
SET PassCodeHash = @Base64Hash
WHERE Username = 'admin';

PRINT 'Updated Admin passcode to: ' + @Base64Hash;
