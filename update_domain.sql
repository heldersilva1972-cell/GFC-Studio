
IF NOT EXISTS (SELECT 1 FROM SystemSettings WHERE Id = 1)
BEGIN
    INSERT INTO SystemSettings (
        Id, 
        PrimaryDomain, 
        LastUpdatedUtc, 
        HostingEnvironment, 
        TrustedDeviceDurationDays, 
        MagicLinkEnabled, 
        EnforceVpn, 
        AccessMode, 
        IdleTimeoutMinutes, 
        AbsoluteSessionMaxMinutes, 
        SafeModeEnabled, 
        EnableOnboarding
    ) VALUES (
        1, 
        'gfc.lovanow.com', 
        GETUTCDATE(),
        'Production', 
        30, 
        1, 
        0, 
        'Hybrid', 
        30, 
        720, 
        0, 
        1
    );
END
ELSE
BEGIN
    UPDATE SystemSettings
    SET PrimaryDomain = 'gfc.lovanow.com',
        LastUpdatedUtc = GETUTCDATE()
    WHERE Id = 1;
END

SELECT PrimaryDomain FROM SystemSettings WHERE Id = 1;
