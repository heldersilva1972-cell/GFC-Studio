# Architectural Fixes - December 30, 2025

Resolved 28+ errors related to circular dependencies and missing types.

## Issue Summary
1. **Circular Dependency**: `GFC.Core` was referencing `GFC.BlazorServer`, while `GFC.BlazorServer` references `GFC.Core`.
   - `KeyCardLifecycleService.cs` (Core) had `using GFC.BlazorServer.Services.Controllers;`.
2. **Missing Reference / Wrong Layer**:
   - `ControllerHealthService` and `ControllerFullSyncService` were placed in `GFC.Core`.
   - These services depend on `IControllerClient` which is defined in `GFC.BlazorServer`.
   - Since `IControllerClient` cannot be easily moved (deep dependencies), the consuming services must be in `GFC.BlazorServer`.

## Changes Made
1. **Moved Services**:
   - `ControllerHealthService.cs` moved from `GFC.Core.Services` to `GFC.BlazorServer.Services`.
   - `ControllerFullSyncService.cs` moved from `GFC.Core.Services` to `GFC.BlazorServer.Services`.
2. **Cleaned References**:
   - Removed `using GFC.BlazorServer...` from `KeyCardLifecycleService.cs`.
3. **Updated Namespaces**:
   - Updated namespaces of moved services to `GFC.BlazorServer.Services`.
4. **Code Improvements**:
   - `ControllerHealthService`: Replaced call to non-existent/invalid `GetStatusAsync` with `PingAsync`.

## Impact
- `GFC.Core` is now pure and free of UI/Host dependencies.
- `GFC.BlazorServer` handles all hardware/infrastructure interactions.
- Circular dependency compilation errors are resolved.
- Cascading errors in `GFC.Data` (unable to find Core types) should be resolved automatically once Core compiles.
