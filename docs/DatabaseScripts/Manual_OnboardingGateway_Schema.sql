-- =============================================
-- Onboarding Gateway Database Enhancements
-- =============================================
-- This script adds optimizations for the public onboarding gateway

USE [ClubMembership]
GO

PRINT 'Starting Onboarding Gateway Database Enhancements...'
GO

-- =============================================
-- 1. Add Index for Token Lookups
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_VpnOnboardingTokens_Token' AND object_id = OBJECT_ID('dbo.VpnOnboardingTokens'))
BEGIN
    CREATE INDEX IX_VpnOnboardingTokens_Token 
    ON dbo.VpnOnboardingTokens(Token);
    PRINT '✓ Created index IX_VpnOnboardingTokens_Token for faster token validation';
END
ELSE
BEGIN
    PRINT '  Index IX_VpnOnboardingTokens_Token already exists';
END
GO

-- =============================================
-- 2. Add Index for User ID Lookups
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_VpnOnboardingTokens_UserId' AND object_id = OBJECT_ID('dbo.VpnOnboardingTokens'))
BEGIN
    CREATE INDEX IX_VpnOnboardingTokens_UserId 
    ON dbo.VpnOnboardingTokens(UserId);
    PRINT '✓ Created index IX_VpnOnboardingTokens_UserId for user lookups';
END
ELSE
BEGIN
    PRINT '  Index IX_VpnOnboardingTokens_UserId already exists';
END
GO

-- =============================================
-- 3. Add Index for Expiry Checks
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_VpnOnboardingTokens_ExpiresAtUtc' AND object_id = OBJECT_ID('dbo.VpnOnboardingTokens'))
BEGIN
    CREATE INDEX IX_VpnOnboardingTokens_ExpiresAtUtc 
    ON dbo.VpnOnboardingTokens(ExpiresAtUtc);
    PRINT '✓ Created index IX_VpnOnboardingTokens_ExpiresAtUtc for expiry checks';
END
ELSE
BEGIN
    PRINT '  Index IX_VpnOnboardingTokens_ExpiresAtUtc already exists';
END
GO

-- =============================================
-- 4. Add Composite Index for Active Token Lookups
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_VpnOnboardingTokens_Token_IsUsed_ExpiresAtUtc' AND object_id = OBJECT_ID('dbo.VpnOnboardingTokens'))
BEGIN
    CREATE INDEX IX_VpnOnboardingTokens_Token_IsUsed_ExpiresAtUtc 
    ON dbo.VpnOnboardingTokens(Token, IsUsed, ExpiresAtUtc);
    PRINT '✓ Created composite index for active token validation';
END
ELSE
BEGIN
    PRINT '  Composite index already exists';
END
GO

-- =============================================
-- 5. Add SystemSettings Fields for Onboarding
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'dbo.SystemSettings') AND name = 'OnboardingGatewayUrl')
BEGIN
    ALTER TABLE dbo.SystemSettings 
    ADD OnboardingGatewayUrl NVARCHAR(500) NULL;
    PRINT '✓ Added OnboardingGatewayUrl column';
END
ELSE
BEGIN
    PRINT '  OnboardingGatewayUrl column already exists';
END
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'dbo.SystemSettings') AND name = 'OnboardingTokenExpiryHours')
BEGIN
    ALTER TABLE dbo.SystemSettings 
    ADD OnboardingTokenExpiryHours INT NOT NULL DEFAULT 48;
    PRINT '✓ Added OnboardingTokenExpiryHours column';
END
ELSE
BEGIN
    PRINT '  OnboardingTokenExpiryHours column already exists';
END
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'dbo.SystemSettings') AND name = 'OnboardingRateLimitPerMinute')
BEGIN
    ALTER TABLE dbo.SystemSettings 
    ADD OnboardingRateLimitPerMinute INT NOT NULL DEFAULT 10;
    PRINT '✓ Added OnboardingRateLimitPerMinute column';
END
ELSE
BEGIN
    PRINT '  OnboardingRateLimitPerMinute column already exists';
END
GO

-- =============================================
-- 6. Set Default Values for Existing Row
-- =============================================
UPDATE dbo.SystemSettings
SET 
    OnboardingGatewayUrl = 'https://setup.gfc.lovanow.com',
    OnboardingTokenExpiryHours = 48,
    OnboardingRateLimitPerMinute = 10
WHERE Id = 1 
  AND OnboardingGatewayUrl IS NULL;

IF @@ROWCOUNT > 0
    PRINT '✓ Set default onboarding configuration values';
ELSE
    PRINT '  Default values already configured';
GO

-- =============================================
-- 7. VpnProfile Database Enhancements
-- =============================================

-- Index for UserID lookups
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_VpnProfiles_UserId' AND object_id = OBJECT_ID('dbo.VpnProfiles'))
BEGIN
    CREATE INDEX IX_VpnProfiles_UserId ON dbo.VpnProfiles(UserId);
    PRINT '✓ Created index IX_VpnProfiles_UserId';
END
GO

-- Index for PublicKey lookups (Critical for WireGuard server integration)
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_VpnProfiles_PublicKey' AND object_id = OBJECT_ID('dbo.VpnProfiles'))
BEGIN
    CREATE INDEX IX_VpnProfiles_PublicKey ON dbo.VpnProfiles(PublicKey);
    PRINT '✓ Created index IX_VpnProfiles_PublicKey';
END
GO

-- Filtered Index for Active Profiles
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_VpnProfiles_Active' AND object_id = OBJECT_ID('dbo.VpnProfiles'))
BEGIN
    CREATE INDEX IX_VpnProfiles_Active ON dbo.VpnProfiles(UserId) WHERE RevokedAt IS NULL;
    PRINT '✓ Created filtered index for active VPN profiles';
END
GO

PRINT ''
PRINT '================================================'
PRINT 'Database Enhancements Complete!'
PRINT '================================================'
PRINT ''
PRINT 'Next Steps:'
PRINT '1. Deploy the onboarding gateway'
PRINT '2. Update Program.cs with rate limiting'
PRINT '3. Verify WireGuard server can query VpnProfiles'
PRINT ''
GO
