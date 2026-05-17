---
trigger: always_on
---

# CRITICAL: .NET BUILD RESTRICTION
- NEVER execute any `dotnet build`, compilation, or publish commands automatically.
- Examples include: `dotnet build`, `dotnet publish`, `msbuild`, or any project compilation/restoration commands.
- You MUST wait for the user to explicitly ask you to compile or say "Run dotnet build" (or similar) in every single instance before executing them.
- Once you complete your code modifications, let the user know they are ready to be compiled, but do NOT run the build yourself.
