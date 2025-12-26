# AGENTS.md - GFC-Studio Development Guide

## Project Overview
GFC-Studio is a [Insert brief description, e.g., Blazor-based simulation dashboard]. 
Current Date: December 2025. Ignore training data prior to 2024 for API syntax.

## Tech Stack Rules
- **Backend:** [e.g., .NET 8, C#, Dynamic MongoDB]
- **Frontend:** [e.g., Blazor Server, HTML, CSS]
- **SDK Rules:** ALWAYS use the latest unified `google-genai` SDK. NEVER use legacy `google-generative-ai` modules.

## Coding Standards
- **Naming:** Use PascalCase for C# classes and methods; camelCase for private fields.
- **Documentation:** Every public method must have XML documentation comments.
- **Structure:** Keep logic in Services; Controllers should only handle routing and basic validation.

## Visual Indicator Requirements
- **NEW/UPDATED Badges:** All new or updated UI elements MUST have a "NEW" or "UPDATED" badge.
- **Video Features:** All video features MUST include visual indicators for stream status (e.g., "LIVE", "OFFLINE", "BUFFERING").

## Build & Test Commands
- **Install:** `dotnet restore`
- **Build:** `dotnet build`
- **Test:** `dotnet test`

## Sync Protocols
- Always run `dotnet build` and verify no errors before committing.
- Commit messages must follow Conventional Commits (e.g., `feat:`, `fix:`, `docs:`).