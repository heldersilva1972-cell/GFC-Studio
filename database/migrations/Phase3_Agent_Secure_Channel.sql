-- ============================================================================
-- Phase 3: Secure Agent Channel - Database Schema
-- ============================================================================
-- Purpose: Support outbound Agent→WebApp gRPC streaming with mTLS
-- Security: Agent authentication, command audit, certificate management
-- ============================================================================

USE [ClubMembership];
GO

-- ============================================================================
-- Table: Agents
-- Purpose: Track registered agents (pending, approved, revoked)
-- ============================================================================

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Agents')
BEGIN
    CREATE TABLE [dbo].[Agents] (
        [AgentId] INT PRIMARY KEY IDENTITY(1,1),
        [AgentName] NVARCHAR(100) NOT NULL,
        [MachineFingerprint] NVARCHAR(64) NOT NULL UNIQUE,
        [CertificateThumbprint] NVARCHAR(64) NULL,
        [Status] NVARCHAR(20) NOT NULL DEFAULT 'Pending', -- Pending, Approved, Revoked
        [ConnectionMode] NVARCHAR(20) NOT NULL DEFAULT 'Outbound', -- Outbound, Local
        [LocalAddress] NVARCHAR(100) NULL,
        [Version] NVARCHAR(50) NULL,
        [OsVersion] NVARCHAR(100) NULL,
        [Location] NVARCHAR(200) NULL,
        [LastSeenUtc] DATETIME2 NULL,
        [LastError] NVARCHAR(MAX) NULL,
        [LastHealthStatus] NVARCHAR(20) NULL, -- Healthy, Degraded, Unhealthy
        [CreatedUtc] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        [ApprovedByUserId] NVARCHAR(450) NULL,
        [ApprovedUtc] DATETIME2 NULL,
        [RevokedUtc] DATETIME2 NULL,
        [RevokedReason] NVARCHAR(500) NULL,
        [RevokedByUserId] NVARCHAR(450) NULL,
        [CertificateExpiresUtc] DATETIME2 NULL,
        [CertificateRenewedUtc] DATETIME2 NULL,
        [Notes] NVARCHAR(MAX) NULL,
        
        CONSTRAINT [FK_Agents_ApprovedBy] FOREIGN KEY ([ApprovedByUserId]) 
            REFERENCES [AspNetUsers]([Id]),
        CONSTRAINT [FK_Agents_RevokedBy] FOREIGN KEY ([RevokedByUserId]) 
            REFERENCES [AspNetUsers]([Id]),
        CONSTRAINT [CK_Agents_Status] CHECK ([Status] IN ('Pending', 'Approved', 'Revoked')),
        CONSTRAINT [CK_Agents_ConnectionMode] CHECK ([ConnectionMode] IN ('Outbound', 'Local'))
    );
    
    CREATE INDEX [IX_Agents_Status] ON [dbo].[Agents]([Status]);
    CREATE INDEX [IX_Agents_LastSeenUtc] ON [dbo].[Agents]([LastSeenUtc] DESC);
    CREATE INDEX [IX_Agents_MachineFingerprint] ON [dbo].[Agents]([MachineFingerprint]);
    CREATE INDEX [IX_Agents_CertificateThumbprint] ON [dbo].[Agents]([CertificateThumbprint]);
    
    PRINT 'Created table: Agents';
END
ELSE
BEGIN
    PRINT 'Table already exists: Agents';
END
GO

-- ============================================================================
-- Table: AgentCommandAudit
-- Purpose: Audit log for all commands sent to agents
-- Security: Track who did what, when, and what happened
-- ============================================================================

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'AgentCommandAudit')
BEGIN
    CREATE TABLE [dbo].[AgentCommandAudit] (
        [AuditId] BIGINT PRIMARY KEY IDENTITY(1,1),
        [CommandId] UNIQUEIDENTIFIER NOT NULL,
        [CorrelationId] UNIQUEIDENTIFIER NULL,
        [AgentId] INT NOT NULL,
        [CommandType] NVARCHAR(50) NOT NULL,
        [TargetControllerSN] NVARCHAR(50) NULL,
        [TargetDoorIndex] INT NULL,
        [InitiatedByUserId] NVARCHAR(450) NOT NULL,
        [InitiatedByUserName] NVARCHAR(256) NULL,
        [ParametersSummary] NVARCHAR(MAX) NULL, -- JSON summary (no secrets)
        [ResultStatus] NVARCHAR(20) NOT NULL, -- Success, Failed, Timeout, Cancelled
        [ResultMessage] NVARCHAR(MAX) NULL,
        [ErrorCode] NVARCHAR(50) NULL,
        [DurationMs] INT NULL,
        [RequiredConfirmation] BIT NOT NULL DEFAULT 0,
        [ConfirmationToken] NVARCHAR(100) NULL,
        [ConfirmationMetadata] NVARCHAR(MAX) NULL, -- JSON: who confirmed, when, etc.
        [RiskLevel] NVARCHAR(20) NULL, -- Low, Medium, High, Critical
        [WasReplayed] BIT NOT NULL DEFAULT 0,
        [CreatedUtc] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        [CompletedUtc] DATETIME2 NULL,
        [TimeoutSeconds] INT NULL,
        
        CONSTRAINT [FK_AgentCommandAudit_Agent] FOREIGN KEY ([AgentId]) 
            REFERENCES [dbo].[Agents]([AgentId]),
        CONSTRAINT [FK_AgentCommandAudit_User] FOREIGN KEY ([InitiatedByUserId]) 
            REFERENCES [AspNetUsers]([Id]),
        CONSTRAINT [CK_AgentCommandAudit_ResultStatus] CHECK ([ResultStatus] IN ('Success', 'Failed', 'Timeout', 'Cancelled'))
    );
    
    CREATE UNIQUE INDEX [IX_AgentCommandAudit_CommandId] ON [dbo].[AgentCommandAudit]([CommandId]);
    CREATE INDEX [IX_AgentCommandAudit_AgentId_CreatedUtc] ON [dbo].[AgentCommandAudit]([AgentId], [CreatedUtc] DESC);
    CREATE INDEX [IX_AgentCommandAudit_ControllerSN] ON [dbo].[AgentCommandAudit]([TargetControllerSN]);
    CREATE INDEX [IX_AgentCommandAudit_UserId] ON [dbo].[AgentCommandAudit]([InitiatedByUserId]);
    CREATE INDEX [IX_AgentCommandAudit_CommandType] ON [dbo].[AgentCommandAudit]([CommandType]);
    CREATE INDEX [IX_AgentCommandAudit_CorrelationId] ON [dbo].[AgentCommandAudit]([CorrelationId]) WHERE [CorrelationId] IS NOT NULL;
    
    PRINT 'Created table: AgentCommandAudit';
END
ELSE
BEGIN
    PRINT 'Table already exists: AgentCommandAudit';
END
GO

-- ============================================================================
-- Table: AgentConnectionLog
-- Purpose: Track agent connection/disconnection events
-- ============================================================================

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'AgentConnectionLog')
BEGIN
    CREATE TABLE [dbo].[AgentConnectionLog] (
        [LogId] BIGINT PRIMARY KEY IDENTITY(1,1),
        [AgentId] INT NOT NULL,
        [EventType] NVARCHAR(20) NOT NULL, -- Connected, Disconnected, Heartbeat, Error
        [Message] NVARCHAR(MAX) NULL,
        [IpAddress] NVARCHAR(50) NULL,
        [Version] NVARCHAR(50) NULL,
        [HealthStatus] NVARCHAR(20) NULL,
        [ErrorCode] NVARCHAR(50) NULL,
        [CreatedUtc] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        
        CONSTRAINT [FK_AgentConnectionLog_Agent] FOREIGN KEY ([AgentId]) 
            REFERENCES [dbo].[Agents]([AgentId]),
        CONSTRAINT [CK_AgentConnectionLog_EventType] CHECK ([EventType] IN ('Connected', 'Disconnected', 'Heartbeat', 'Error', 'StatusUpdate'))
    );
    
    CREATE INDEX [IX_AgentConnectionLog_AgentId_CreatedUtc] ON [dbo].[AgentConnectionLog]([AgentId], [CreatedUtc] DESC);
    CREATE INDEX [IX_AgentConnectionLog_EventType] ON [dbo].[AgentConnectionLog]([EventType]);
    
    PRINT 'Created table: AgentConnectionLog';
END
ELSE
BEGIN
    PRINT 'Table already exists: AgentConnectionLog';
END
GO

-- ============================================================================
-- Table: AgentCertificates
-- Purpose: Track certificate lifecycle (issuance, renewal, revocation)
-- ============================================================================

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'AgentCertificates')
BEGIN
    CREATE TABLE [dbo].[AgentCertificates] (
        [CertificateId] INT PRIMARY KEY IDENTITY(1,1),
        [AgentId] INT NOT NULL,
        [Thumbprint] NVARCHAR(64) NOT NULL UNIQUE,
        [SerialNumber] NVARCHAR(100) NULL,
        [SubjectDN] NVARCHAR(500) NULL,
        [IssuerDN] NVARCHAR(500) NULL,
        [NotBefore] DATETIME2 NOT NULL,
        [NotAfter] DATETIME2 NOT NULL,
        [Status] NVARCHAR(20) NOT NULL DEFAULT 'Active', -- Active, Expired, Revoked
        [CertificatePem] NVARCHAR(MAX) NULL, -- Public cert only
        [IssuedUtc] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        [IssuedByUserId] NVARCHAR(450) NULL,
        [RevokedUtc] DATETIME2 NULL,
        [RevokedReason] NVARCHAR(500) NULL,
        [RevokedByUserId] NVARCHAR(450) NULL,
        
        CONSTRAINT [FK_AgentCertificates_Agent] FOREIGN KEY ([AgentId]) 
            REFERENCES [dbo].[Agents]([AgentId]),
        CONSTRAINT [FK_AgentCertificates_IssuedBy] FOREIGN KEY ([IssuedByUserId]) 
            REFERENCES [AspNetUsers]([Id]),
        CONSTRAINT [FK_AgentCertificates_RevokedBy] FOREIGN KEY ([RevokedByUserId]) 
            REFERENCES [AspNetUsers]([Id]),
        CONSTRAINT [CK_AgentCertificates_Status] CHECK ([Status] IN ('Active', 'Expired', 'Revoked'))
    );
    
    CREATE INDEX [IX_AgentCertificates_AgentId] ON [dbo].[AgentCertificates]([AgentId]);
    CREATE INDEX [IX_AgentCertificates_Status] ON [dbo].[AgentCertificates]([Status]);
    CREATE INDEX [IX_AgentCertificates_NotAfter] ON [dbo].[AgentCertificates]([NotAfter]);
    
    PRINT 'Created table: AgentCertificates';
END
ELSE
BEGIN
    PRINT 'Table already exists: AgentCertificates';
END
GO

-- ============================================================================
-- Sample Data (for development/testing)
-- ============================================================================

PRINT '';
PRINT '============================================================================';
PRINT 'Phase 3: Secure Agent Channel - Schema Created Successfully';
PRINT '============================================================================';
PRINT 'Tables created:';
PRINT '  - Agents (agent registration and status)';
PRINT '  - AgentCommandAudit (command execution audit log)';
PRINT '  - AgentConnectionLog (connection events)';
PRINT '  - AgentCertificates (certificate lifecycle)';
PRINT '';
PRINT 'Next steps:';
PRINT '  1. Build GFC.Shared.Agent library';
PRINT '  2. Implement AgentGateway gRPC service in WebApp';
PRINT '  3. Build GFC.AgentService Windows Service';
PRINT '  4. Create Admin UI for agent management';
PRINT '============================================================================';
GO
