-- Test script to manually verify passkey registration would work
-- This simulates what the application does

DECLARE @TestUserId INT = 1; -- Change this to the actual user ID trying to register
DECLARE @TestCredentialId NVARCHAR(500) = 'TEST_CREDENTIAL_' + CONVERT(NVARCHAR(50), NEWID());
DECLARE @TestPublicKey NVARCHAR(MAX) = 'TEST_PUBLIC_KEY_DATA';
DECLARE @TestFriendlyName NVARCHAR(200) = 'Test Device Registration';

-- Check if user exists
IF EXISTS (SELECT 1 FROM AppUsers WHERE UserId = @TestUserId)
BEGIN
    PRINT 'User exists: UserId = ' + CAST(@TestUserId AS NVARCHAR(10));
    
    -- Try to insert a test passkey
    BEGIN TRY
        INSERT INTO UserPasskeys (
            UserId,
            CredentialId,
            PublicKey,
            FriendlyName,
            UserHandle,
            SignatureCounter,
            AttestationFormat,
            CreatedAtUtc,
            AAGUID
        )
        VALUES (
            @TestUserId,
            @TestCredentialId,
            @TestPublicKey,
            @TestFriendlyName,
            CONVERT(NVARCHAR(500), @TestUserId),
            0,
            'none',
            GETUTCDATE(),
            '00000000-0000-0000-0000-000000000000'
        );
        
        PRINT 'SUCCESS: Test passkey inserted successfully';
        PRINT 'Credential ID: ' + @TestCredentialId;
        
        -- Clean up test data
        DELETE FROM UserPasskeys WHERE CredentialId = @TestCredentialId;
        PRINT 'Test passkey cleaned up';
    END TRY
    BEGIN CATCH
        PRINT 'ERROR: Failed to insert test passkey';
        PRINT 'Error Message: ' + ERROR_MESSAGE();
        PRINT 'Error Number: ' + CAST(ERROR_NUMBER() AS NVARCHAR(10));
    END CATCH
END
ELSE
BEGIN
    PRINT 'ERROR: User does not exist with UserId = ' + CAST(@TestUserId AS NVARCHAR(10));
    PRINT 'Please update @TestUserId to a valid user ID';
END
