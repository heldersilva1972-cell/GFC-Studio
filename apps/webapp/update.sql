DECLARE @InputString VARCHAR(100) = '851181';
DECLARE @Hash VARBINARY(32) = HASHBYTES('SHA2_256', @InputString);
DECLARE @Base64Hash VARCHAR(100) = CAST('' AS XML).value('xs:base64Binary(sql:variable("@Hash"))', 'VARCHAR(100)');

-- Check if user exists, if not create it
IF NOT EXISTS (SELECT 1 FROM AppUsers WHERE Username = 'admin')
BEGIN
    INSERT INTO AppUsers (Username, PasswordHash, IsAdmin, IsActive, CreatedDate, PasswordChangeRequired, PassCodeHash)
    VALUES ('admin', 'eJIaLDaCl5IDkjkQwmiA6oDBC3GUzDhnD15xRjP4bjo=', 1, 1, GETUTCDATE(), 0, @Base64Hash);
    PRINT 'Admin user created with passcode 851181';
END
ELSE
BEGIN
    UPDATE AppUsers
    SET PassCodeHash = @Base64Hash
    WHERE Username = 'admin';
    PRINT 'Admin passcode updated to 851181';
END

