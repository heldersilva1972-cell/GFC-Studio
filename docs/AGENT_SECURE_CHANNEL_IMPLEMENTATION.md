# Phase 3: Secure Agent Channel Implementation Plan

## Overview
Implement outbound-only Agent→WebApp secure command channel using gRPC streaming with mTLS for controller operations across different networks.

## Architecture

```
┌─────────────────────────────────────────────────────────────────┐
│ Network A (Web App)                                              │
│                                                                   │
│  ┌──────────────────────────────────────────────────────────┐   │
│  │ GFC.BlazorServer                                          │   │
│  │  ├─ AgentGateway (gRPC Server)                           │   │
│  │  │   ├─ mTLS validation                                  │   │
│  │  │   ├─ Agent registry (online/offline)                  │   │
│  │  │   ├─ Command dispatcher                               │   │
│  │  │   └─ Audit logger                                     │   │
│  │  ├─ Admin UI (Agent Management)                          │   │
│  │  └─ Database (Agent records, audit logs)                 │   │
│  └──────────────────────────────────────────────────────────┘   │
│                           ▲                                       │
│                           │ Outbound gRPC/TLS (mTLS)            │
└───────────────────────────┼───────────────────────────────────────┘
                            │
                            │ (Internet / WAN)
                            │
┌───────────────────────────┼───────────────────────────────────────┐
│ Network B (Controller LAN)│                                       │
│                           │                                       │
│  ┌────────────────────────▼──────────────────────────────────┐   │
│  │ GFC.AgentService (Windows Service)                        │   │
│  │  ├─ Outbound gRPC client                                 │   │
│  │  ├─ Certificate management (CSR, renewal)                │   │
│  │  ├─ Command executor (allowlist only)                    │   │
│  │  ├─ Controller client (WG3000 protocol)                  │   │
│  │  └─ Local audit log                                      │   │
│  └───────────────────────────────────────────────────────────┘   │
│                           │                                       │
│                           │ LAN (UDP/TCP)                        │
│                           ▼                                       │
│  ┌───────────────────────────────────────────────────────────┐   │
│  │ WG3000 Controllers                                        │   │
│  └───────────────────────────────────────────────────────────┘   │
└───────────────────────────────────────────────────────────────────┘
```

## Security Model

### Transport Security
- **TLS 1.3** for all communications
- **mTLS** (mutual authentication):
  - Agent presents client certificate
  - WebApp presents server certificate
  - Both validate each other

### Authentication & Authorization
- **Agent Identity**: Client certificate thumbprint
- **Allowlist**: Database-stored approved agent thumbprints
- **Command Allowlist**: Only specific operations permitted
- **User Context**: Commands include originating user ID for audit

### Command Allowlist
```csharp
public enum AllowedCommand
{
    // Safe operations
    OpenDoor,
    GetRunStatus,
    GetEventsByIndex,
    SyncTime,
    
    // Privilege management
    PrivilegeAdd,
    PrivilegeUpdate,
    PrivilegeDelete,
    
    // Dangerous operations (require confirmation)
    BulkUpload,
    ClearAllCards,
    ReadFlash,
    WriteFlash,
    ReadFRAM,
    WriteFRAM,
    NetworkConfig,
    Reboot
}
```

## Implementation Phases

### Phase 1: Foundation (Week 1)
- [ ] Define gRPC `.proto` contract
- [ ] Create `GFC.AgentService` project structure
- [ ] Create `GFC.Shared.Agent` contracts library
- [ ] Database schema for agents and audit logs
- [ ] Basic certificate infrastructure

### Phase 2: Agent Service (Week 2)
- [ ] Windows Service scaffolding
- [ ] Outbound gRPC client with reconnection logic
- [ ] CSR generation and certificate enrollment
- [ ] Command executor with allowlist enforcement
- [ ] Controller protocol integration
- [ ] Local logging and error handling

### Phase 3: WebApp Integration (Week 3)
- [ ] AgentGateway gRPC server endpoint
- [ ] mTLS validation middleware
- [ ] Agent connection registry
- [ ] Command dispatcher with timeout/retry
- [ ] Audit logging to database
- [ ] Health monitoring

### Phase 4: Onboarding Flow (Week 4)
- [ ] Agent registration endpoint
- [ ] Pending agent approval UI
- [ ] Certificate issuance/signing
- [ ] Auto-renewal logic
- [ ] Revocation mechanism

### Phase 5: Admin UI (Week 5)
- [ ] Agent list page (online/offline status)
- [ ] Pending approvals interface
- [ ] Agent details and health
- [ ] Audit log viewer
- [ ] Connection mode settings (Outbound/Local)
- [ ] Certificate management UI

### Phase 6: Installer & Deployment (Week 6)
- [ ] MSI/EXE installer package
- [ ] Configuration wizard
- [ ] Auto-update mechanism
- [ ] Deployment documentation
- [ ] Troubleshooting guide

### Phase 7: Testing & Hardening (Week 7)
- [ ] Security tests (invalid certs, replay attacks)
- [ ] Failover and reconnection tests
- [ ] Performance and load tests
- [ ] Penetration testing
- [ ] Documentation review

## Database Schema

### Agents Table
```sql
CREATE TABLE Agents (
    AgentId INT PRIMARY KEY IDENTITY,
    AgentName NVARCHAR(100) NOT NULL,
    MachineFingerprint NVARCHAR(64) NOT NULL UNIQUE,
    CertificateThumbprint NVARCHAR(64) NULL,
    Status NVARCHAR(20) NOT NULL, -- Pending, Approved, Revoked
    ConnectionMode NVARCHAR(20) NOT NULL DEFAULT 'Outbound', -- Outbound, Local
    LocalAddress NVARCHAR(100) NULL,
    Version NVARCHAR(50) NULL,
    LastSeenUtc DATETIME2 NULL,
    LastError NVARCHAR(MAX) NULL,
    CreatedUtc DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    ApprovedByUserId INT NULL,
    ApprovedUtc DATETIME2 NULL,
    RevokedUtc DATETIME2 NULL,
    RevokedReason NVARCHAR(500) NULL,
    CONSTRAINT FK_Agents_ApprovedBy FOREIGN KEY (ApprovedByUserId) REFERENCES AspNetUsers(Id)
);
```

### AgentCommandAudit Table
```sql
CREATE TABLE AgentCommandAudit (
    AuditId BIGINT PRIMARY KEY IDENTITY,
    CommandId UNIQUEIDENTIFIER NOT NULL,
    CorrelationId UNIQUEIDENTIFIER NULL,
    AgentId INT NOT NULL,
    CommandType NVARCHAR(50) NOT NULL,
    TargetControllerSN NVARCHAR(50) NULL,
    TargetDoorIndex INT NULL,
    InitiatedByUserId INT NOT NULL,
    ParametersSummary NVARCHAR(MAX) NULL,
    ResultStatus NVARCHAR(20) NOT NULL, -- Success, Failed, Timeout
    ResultMessage NVARCHAR(MAX) NULL,
    DurationMs INT NULL,
    RequiredConfirmation BIT NOT NULL DEFAULT 0,
    ConfirmationMetadata NVARCHAR(MAX) NULL,
    CreatedUtc DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    CompletedUtc DATETIME2 NULL,
    CONSTRAINT FK_AgentCommandAudit_Agent FOREIGN KEY (AgentId) REFERENCES Agents(AgentId),
    CONSTRAINT FK_AgentCommandAudit_User FOREIGN KEY (InitiatedByUserId) REFERENCES AspNetUsers(Id)
);

CREATE INDEX IX_AgentCommandAudit_CommandId ON AgentCommandAudit(CommandId);
CREATE INDEX IX_AgentCommandAudit_AgentId_CreatedUtc ON AgentCommandAudit(AgentId, CreatedUtc DESC);
CREATE INDEX IX_AgentCommandAudit_ControllerSN ON AgentCommandAudit(TargetControllerSN);
```

## gRPC Contract (agent.proto)

```protobuf
syntax = "proto3";

package gfc.agent;

option csharp_namespace = "GFC.Shared.Agent";

service AgentGateway {
  // Bidirectional streaming for commands and results
  rpc CommandStream (stream AgentMessage) returns (stream GatewayMessage);
  
  // Agent registration (before mTLS established)
  rpc RegisterAgent (RegistrationRequest) returns (RegistrationResponse);
}

message AgentMessage {
  oneof payload {
    Heartbeat heartbeat = 1;
    CommandResult result = 2;
    StatusUpdate status = 3;
  }
}

message GatewayMessage {
  oneof payload {
    CommandRequest command = 1;
    HeartbeatAck heartbeat_ack = 2;
    ConfigUpdate config = 3;
  }
}

message Heartbeat {
  string agent_id = 1;
  string version = 2;
  int64 timestamp_utc = 3;
}

message HeartbeatAck {
  int64 timestamp_utc = 1;
}

message CommandRequest {
  string command_id = 1;
  string correlation_id = 2;
  string command_type = 3;
  bytes payload = 4;
  string requested_by_user_id = 5;
  int64 timestamp_utc = 6;
  int32 timeout_seconds = 7;
}

message CommandResult {
  string command_id = 1;
  string correlation_id = 2;
  bool success = 3;
  string error_code = 4;
  string error_message = 5;
  bytes result_payload = 6;
  int64 timestamp_utc = 7;
  int32 duration_ms = 8;
}

message StatusUpdate {
  string agent_id = 1;
  string status = 2; // Online, Degraded, Error
  string message = 3;
  int64 timestamp_utc = 4;
}

message ConfigUpdate {
  int32 heartbeat_interval_seconds = 1;
  repeated string allowed_commands = 2;
}

message RegistrationRequest {
  string machine_fingerprint = 1;
  string agent_name = 2;
  string version = 3;
  string csr_pem = 4; // Certificate Signing Request
  string location = 5;
}

message RegistrationResponse {
  bool success = 1;
  string agent_id = 2;
  string message = 3;
  string error_code = 4;
}
```

## Key Features

### 1. Automated Onboarding
1. Admin downloads `GFC-Agent-Setup.msi`
2. Runs installer on Agent PC (controller LAN)
3. Installer prompts for:
   - Agent Name/Location
   - WebApp URL
4. Agent generates keypair + CSR locally
5. Agent phones home to `/agent/register`
6. Admin sees "Pending Agent" in UI
7. Admin clicks "Approve"
8. WebApp signs cert or adds thumbprint to allowlist
9. Agent retrieves cert and establishes mTLS connection
10. Status changes to "Online"

### 2. Command Execution Flow
1. User clicks "Open Door" in WebApp UI
2. WebApp validates user permissions
3. WebApp creates `CommandRequest` with unique `CommandId`
4. WebApp sends command over gRPC stream to Agent
5. Agent validates command is in allowlist
6. Agent checks for duplicate `CommandId` (replay protection)
7. Agent executes WG3000 protocol operation
8. Agent returns `CommandResult`
9. WebApp logs audit record
10. UI shows result to user

### 3. Dangerous Operation Confirmation
```csharp
[DangerousOperation]
public async Task<IActionResult> ClearAllCards(string controllerSN)
{
    // UI must show confirmation dialog first
    if (!Request.Headers.TryGetValue("X-Confirmation-Token", out var token))
    {
        return BadRequest("Confirmation required");
    }
    
    // Validate confirmation token
    // ... dispatch command with confirmation metadata
}
```

### 4. Offline Handling
- WebApp tracks `LastSeenUtc` for each agent
- If agent offline > 2 minutes: status = "Offline"
- Controller operations disabled in UI with clear message
- Automatic reconnection when agent comes back online

### 5. Future Same-LAN Mode
```csharp
public enum AgentConnectionMode
{
    Outbound,  // Default: Agent connects to WebApp (works across networks)
    Local      // Optional: WebApp connects to Agent (same LAN only)
}
```

Admin UI shows:
- ✅ **Outbound Mode (Recommended)**: Works across any network. Agent maintains connection to WebApp.
- ⚠️ **Local Mode**: Only use if WebApp and Agent are on the same LAN. Requires firewall configuration.

## Security Checklist

- [ ] No inbound ports on controller LAN
- [ ] mTLS enforced (both client and server certs)
- [ ] Certificate thumbprint allowlist in database
- [ ] Command allowlist enforced
- [ ] Replay protection (CommandId deduplication)
- [ ] Rate limiting (per-user, per-command, global)
- [ ] Audit logging (all commands, who/what/when/result)
- [ ] Dangerous operations require explicit confirmation
- [ ] Secrets never logged
- [ ] Certificate rotation supported
- [ ] Revocation immediate
- [ ] Offline-safe behavior
- [ ] Error messages don't leak sensitive info

## Next Steps

1. **Create gRPC contract** (`agent.proto`)
2. **Create shared library** (`GFC.Shared.Agent`)
3. **Create Agent service project** (`GFC.AgentService`)
4. **Database migration** for Agents and AgentCommandAudit tables
5. **Implement AgentGateway** in WebApp
6. **Build Admin UI** for agent management

## Documentation Requirements

- [ ] Architecture diagram
- [ ] Installation guide (non-technical)
- [ ] Approval workflow guide
- [ ] Troubleshooting guide
- [ ] Security model documentation
- [ ] Certificate lifecycle guide
- [ ] Same-LAN transition guide

---

**Status**: Planning Complete  
**Next Action**: Begin Phase 1 implementation  
**Estimated Timeline**: 7 weeks  
**Risk Level**: High (security-critical)
