-- [MANUAL REPAIR] Add Tax Identifiers for 2026 Payroll Compliance
-- Adds EAN/FEIN to SystemSettings and SSN to AppUsers

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[SystemSettings]') AND name = 'MaEmployerAccountNumber')
BEGIN
    ALTER TABLE [SystemSettings] ADD [MaEmployerAccountNumber] NVARCHAR(MAX) NULL;
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[SystemSettings]') AND name = 'FederalEmployerIdNumber')
BEGIN
    ALTER TABLE [SystemSettings] ADD [FederalEmployerIdNumber] NVARCHAR(MAX) NULL;
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[AppUsers]') AND name = 'SocialSecurityNumber')
BEGIN
    ALTER TABLE [AppUsers] ADD [SocialSecurityNumber] NVARCHAR(MAX) NULL;
END
GO
