$sourcePath = "apps/GFC-Pos-Standalone/GFC.Pos.UI/Pages/PosTerminal.razor"
$destPath = "apps/webapp/GFC.BlazorServer/Components/Pages/Admin/Pos/PosTerminal.razor"

Write-Host "Reading source file: $sourcePath"
$content = [System.IO.File]::ReadAllText($sourcePath)

Write-Host "Adapting top imports and directives..."

# Find the end of the imports section (before <style>)
$styleIndex = $content.IndexOf("<style>")
if ($styleIndex -lt 0) {
    Write-Error "Could not find <style> tag in source file!"
    exit 1
}

# Extract the body starting from <style>
$body = $content.Substring($styleIndex)

# Prepare the new headers
$headers = @"
@page "/admin/pos-terminal"
@using Microsoft.AspNetCore.Authorization
@using Microsoft.EntityFrameworkCore
@using GFC.BlazorServer.Data
@using GFC.Core.Models
@using GFC.Core.DTOs
@using GFC.Core.Interfaces
@using GFC.Pos.UI.Services
@using global::System.Text.Json
@using Microsoft.AspNetCore.Components
@inject IJSRuntime JSRuntime
@inject NavigationManager Navigation
@inject IPosTerminalService PosService
@inject IToastService Toast
@inject ConnectivityService Connectivity
@inject IVersionService VersionService
@inject IStationSettingsService StationSettings
@inject IPrinterService PrinterService
@inject IPrinterConfigService PrinterConfig
@inject IDbContextFactory<GfcDbContext> DbFactory
@inject HttpClient Http
@implements IDisposable
@attribute [Authorize]

"@

# Insert the bezel pos-container CSS inside the <style> tag
$bezelStyle = @"
<style>
    /* Sleek physical slate bezel layout for the administrator simulator */
    .pos-container {
        width: 100vw;
        height: 53.33vw; /* 8 / 15 = 53.33% aspect ratio */
        max-height: 100vh;
        max-width: 187.5vh; /* 15 / 8 = 1.875 ratio */
        position: fixed;
        top: 50%;
        left: 50%;
        transform: translate(-50%, -50%);
        background: #0f172a;
        color: #f8fafc;
        display: flex;
        flex-direction: column;
        font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
        box-shadow: 0 25px 50px -12px rgba(0, 0, 0, 0.65);
        overflow: hidden;
        border: 12px solid #1e293b;
        border-radius: 16px;
    }
"@

$adaptedBody = $body.Replace("<style>", $bezelStyle)

$finalContent = $headers + $adaptedBody

Write-Host "Writing adapted content to: $destPath"
[System.IO.File]::WriteAllText($destPath, $finalContent)

Write-Host "Copy and adaptation completed successfully!"
