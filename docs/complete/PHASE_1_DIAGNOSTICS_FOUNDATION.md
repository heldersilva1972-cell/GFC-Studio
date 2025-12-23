# Phase 1: System Diagnostics Foundation & Core Modernization

## Overview
This phase focuses on modernizing the existing System Diagnostics page with a beautiful, modern UI featuring animations, real-time updates, and reusable components. This is the foundation for all future diagnostic features.

---

## Goals
- ✅ Transform the basic diagnostics page into a premium, modern dashboard
- ✅ Create a reusable component library for diagnostic displays
- ✅ Implement smooth animations and visual effects
- ✅ Add real-time auto-refresh functionality
- ✅ Expand the data model to support enhanced diagnostics

---

## Current State

### Existing Files
1. **`Components/Pages/SystemDiagnostics.razor`** - Basic page with 3 static cards
2. **`Services/DiagnosticsService.cs`** - Service that fetches basic diagnostics
3. **`Models/SystemDiagnosticsInfo.cs`** - Basic model with limited properties

### Current Features
- Shows Application info (Environment, Version)
- Shows Database info (Provider, Server, Database, Connection)
- Shows Agent API info (Base URL, Reachability)
- Static display with no animations
- No auto-refresh
- Basic Bootstrap styling

---

## Implementation Tasks

### **Task 1: Create Design System** 🎨

#### 1.1 Create CSS File for Diagnostics
**File**: `wwwroot/css/diagnostics.css`

**Content**:
```css
/* ============================================
   GFC System Diagnostics - Design System
   ============================================ */

:root {
    /* Status Colors - Vibrant Gradients */
    --status-healthy-gradient: linear-gradient(135deg, #10b981 0%, #059669 100%);
    --status-warning-gradient: linear-gradient(135deg, #f59e0b 0%, #d97706 100%);
    --status-error-gradient: linear-gradient(135deg, #ef4444 0%, #dc2626 100%);
    --status-critical-gradient: linear-gradient(135deg, #dc2626 0%, #991b1b 100%);
    --status-unknown-gradient: linear-gradient(135deg, #6b7280 0%, #4b5563 100%);
    
    /* Accent Colors */
    --accent-primary: #667eea;
    --accent-secondary: #764ba2;
    --accent-success: #10b981;
    --accent-warning: #f59e0b;
    --accent-danger: #ef4444;
    --accent-info: #3b82f6;
    
    /* Backgrounds */
    --bg-card: rgba(255, 255, 255, 0.05);
    --bg-card-hover: rgba(255, 255, 255, 0.1);
    --bg-glass: rgba(255, 255, 255, 0.08);
    --bg-glass-border: rgba(255, 255, 255, 0.18);
    
    /* Shadows */
    --shadow-sm: 0 2px 4px rgba(0, 0, 0, 0.1);
    --shadow-md: 0 4px 6px rgba(0, 0, 0, 0.1);
    --shadow-lg: 0 10px 15px rgba(0, 0, 0, 0.1);
    --shadow-xl: 0 20px 25px rgba(0, 0, 0, 0.15);
    --shadow-glow-primary: 0 0 20px rgba(102, 126, 234, 0.3);
    --shadow-glow-success: 0 0 20px rgba(16, 185, 129, 0.3);
    --shadow-glow-danger: 0 0 20px rgba(239, 68, 68, 0.3);
    
    /* Spacing */
    --spacing-xs: 0.25rem;
    --spacing-sm: 0.5rem;
    --spacing-md: 1rem;
    --spacing-lg: 1.5rem;
    --spacing-xl: 2rem;
    
    /* Border Radius */
    --radius-sm: 0.375rem;
    --radius-md: 0.5rem;
    --radius-lg: 0.75rem;
    --radius-xl: 1rem;
    
    /* Transitions */
    --transition-fast: 150ms ease-in-out;
    --transition-normal: 300ms ease-in-out;
    --transition-slow: 500ms ease-in-out;
}

/* ============================================
   Animations
   ============================================ */

@keyframes pulse {
    0%, 100% { opacity: 1; }
    50% { opacity: 0.6; }
}

@keyframes shimmer {
    0% { background-position: -1000px 0; }
    100% { background-position: 1000px 0; }
}

@keyframes fadeIn {
    from { 
        opacity: 0; 
        transform: translateY(20px); 
    }
    to { 
        opacity: 1; 
        transform: translateY(0); 
    }
}

@keyframes fadeInScale {
    from { 
        opacity: 0; 
        transform: scale(0.95); 
    }
    to { 
        opacity: 1; 
        transform: scale(1); 
    }
}

@keyframes slideInRight {
    from { 
        opacity: 0; 
        transform: translateX(20px); 
    }
    to { 
        opacity: 1; 
        transform: translateX(0); 
    }
}

@keyframes countUp {
    from { transform: scale(1.2); opacity: 0.5; }
    to { transform: scale(1); opacity: 1; }
}

@keyframes spin {
    from { transform: rotate(0deg); }
    to { transform: rotate(360deg); }
}

@keyframes glow {
    0%, 100% { box-shadow: var(--shadow-glow-primary); }
    50% { box-shadow: 0 0 30px rgba(102, 126, 234, 0.5); }
}

/* ============================================
   Diagnostic Page Layout
   ============================================ */

.diagnostics-page {
    animation: fadeIn 0.5s ease-out;
}

.diagnostics-header {
    display: flex;
    justify-content: space-between;
    align-items: center;
    margin-bottom: var(--spacing-xl);
    animation: slideInRight 0.5s ease-out;
}

.diagnostics-title {
    font-size: 2rem;
    font-weight: 700;
    background: linear-gradient(135deg, var(--accent-primary) 0%, var(--accent-secondary) 100%);
    -webkit-background-clip: text;
    -webkit-text-fill-color: transparent;
    background-clip: text;
    margin: 0;
}

.diagnostics-actions {
    display: flex;
    gap: var(--spacing-md);
    align-items: center;
}

.last-updated {
    font-size: 0.875rem;
    color: #9ca3af;
    display: flex;
    align-items: center;
    gap: var(--spacing-sm);
}

.last-updated i {
    font-size: 1rem;
}

.last-updated.refreshing {
    color: var(--accent-primary);
}

.last-updated.refreshing i {
    animation: spin 1s linear infinite;
}

/* ============================================
   Metric Cards
   ============================================ */

.metric-card {
    background: var(--bg-glass);
    border: 1px solid var(--bg-glass-border);
    border-radius: var(--radius-lg);
    padding: var(--spacing-lg);
    backdrop-filter: blur(10px);
    transition: all var(--transition-normal);
    animation: fadeInScale 0.5s ease-out;
    height: 100%;
    display: flex;
    flex-direction: column;
    position: relative;
    overflow: hidden;
}

.metric-card::before {
    content: '';
    position: absolute;
    top: 0;
    left: 0;
    right: 0;
    height: 3px;
    background: linear-gradient(90deg, var(--accent-primary), var(--accent-secondary));
    opacity: 0;
    transition: opacity var(--transition-normal);
}

.metric-card:hover {
    transform: translateY(-4px);
    box-shadow: var(--shadow-xl);
    border-color: rgba(255, 255, 255, 0.3);
}

.metric-card:hover::before {
    opacity: 1;
}

.metric-card.clickable {
    cursor: pointer;
}

.metric-card.clickable:active {
    transform: translateY(-2px);
}

/* Card Sizes */
.metric-card.size-small {
    padding: var(--spacing-md);
}

.metric-card.size-medium {
    padding: var(--spacing-lg);
}

.metric-card.size-large {
    padding: var(--spacing-xl);
}

/* Card Header */
.metric-card-header {
    display: flex;
    justify-content: space-between;
    align-items: flex-start;
    margin-bottom: var(--spacing-md);
}

.metric-icon {
    width: 48px;
    height: 48px;
    border-radius: var(--radius-md);
    display: flex;
    align-items: center;
    justify-content: center;
    font-size: 1.5rem;
    background: var(--bg-card);
    transition: all var(--transition-normal);
}

.metric-card:hover .metric-icon {
    transform: scale(1.1);
}

.metric-icon.primary {
    background: linear-gradient(135deg, var(--accent-primary), var(--accent-secondary));
    color: white;
}

.metric-icon.success {
    background: var(--status-healthy-gradient);
    color: white;
}

.metric-icon.warning {
    background: var(--status-warning-gradient);
    color: white;
}

.metric-icon.danger {
    background: var(--status-error-gradient);
    color: white;
}

.metric-icon.info {
    background: linear-gradient(135deg, #3b82f6, #2563eb);
    color: white;
}

/* Card Content */
.metric-content {
    flex: 1;
}

.metric-label {
    font-size: 0.875rem;
    color: #9ca3af;
    font-weight: 500;
    text-transform: uppercase;
    letter-spacing: 0.05em;
    margin-bottom: var(--spacing-sm);
}

.metric-value {
    font-size: 2rem;
    font-weight: 700;
    color: #ffffff;
    display: flex;
    align-items: baseline;
    gap: var(--spacing-sm);
    margin-bottom: var(--spacing-sm);
}

.metric-value.animated {
    animation: countUp 0.5s ease-out;
}

.metric-unit {
    font-size: 1rem;
    color: #9ca3af;
    font-weight: 400;
}

.metric-subtext {
    font-size: 0.875rem;
    color: #6b7280;
    display: flex;
    align-items: center;
    gap: var(--spacing-xs);
}

.metric-subtext i {
    font-size: 0.75rem;
}

.metric-subtext.positive {
    color: var(--accent-success);
}

.metric-subtext.negative {
    color: var(--accent-danger);
}

/* ============================================
   Status Badges
   ============================================ */

.status-badge {
    display: inline-flex;
    align-items: center;
    gap: var(--spacing-sm);
    padding: var(--spacing-xs) var(--spacing-md);
    border-radius: 9999px;
    font-size: 0.75rem;
    font-weight: 600;
    text-transform: uppercase;
    letter-spacing: 0.05em;
    transition: all var(--transition-normal);
}

.status-badge .status-dot {
    width: 8px;
    height: 8px;
    border-radius: 50%;
    animation: pulse 2s ease-in-out infinite;
}

.status-badge.healthy {
    background: rgba(16, 185, 129, 0.1);
    color: #10b981;
    border: 1px solid rgba(16, 185, 129, 0.3);
}

.status-badge.healthy .status-dot {
    background: #10b981;
    box-shadow: 0 0 10px rgba(16, 185, 129, 0.5);
}

.status-badge.warning {
    background: rgba(245, 158, 11, 0.1);
    color: #f59e0b;
    border: 1px solid rgba(245, 158, 11, 0.3);
}

.status-badge.warning .status-dot {
    background: #f59e0b;
    box-shadow: 0 0 10px rgba(245, 158, 11, 0.5);
}

.status-badge.error {
    background: rgba(239, 68, 68, 0.1);
    color: #ef4444;
    border: 1px solid rgba(239, 68, 68, 0.3);
}

.status-badge.error .status-dot {
    background: #ef4444;
    box-shadow: 0 0 10px rgba(239, 68, 68, 0.5);
}

.status-badge.critical {
    background: rgba(220, 38, 38, 0.1);
    color: #dc2626;
    border: 1px solid rgba(220, 38, 38, 0.3);
    animation: glow 2s ease-in-out infinite;
}

.status-badge.critical .status-dot {
    background: #dc2626;
    box-shadow: 0 0 10px rgba(220, 38, 38, 0.5);
}

.status-badge.unknown {
    background: rgba(107, 114, 128, 0.1);
    color: #6b7280;
    border: 1px solid rgba(107, 114, 128, 0.3);
}

.status-badge.unknown .status-dot {
    background: #6b7280;
}

/* ============================================
   Skeleton Loaders
   ============================================ */

.skeleton-loader {
    background: linear-gradient(
        90deg,
        rgba(255, 255, 255, 0.05) 25%,
        rgba(255, 255, 255, 0.1) 50%,
        rgba(255, 255, 255, 0.05) 75%
    );
    background-size: 1000px 100%;
    animation: shimmer 2s infinite;
    border-radius: var(--radius-sm);
}

.skeleton-text {
    height: 1rem;
    margin-bottom: var(--spacing-sm);
}

.skeleton-text.large {
    height: 2rem;
}

.skeleton-text.small {
    height: 0.75rem;
}

.skeleton-circle {
    width: 48px;
    height: 48px;
    border-radius: 50%;
}

/* ============================================
   Buttons
   ============================================ */

.btn-diagnostic {
    display: inline-flex;
    align-items: center;
    gap: var(--spacing-sm);
    padding: var(--spacing-sm) var(--spacing-lg);
    border-radius: var(--radius-md);
    font-weight: 600;
    font-size: 0.875rem;
    transition: all var(--transition-normal);
    border: none;
    cursor: pointer;
    text-decoration: none;
}

.btn-diagnostic:hover {
    transform: translateY(-2px);
    box-shadow: var(--shadow-md);
}

.btn-diagnostic:active {
    transform: translateY(0);
}

.btn-diagnostic.primary {
    background: linear-gradient(135deg, var(--accent-primary), var(--accent-secondary));
    color: white;
}

.btn-diagnostic.primary:hover {
    box-shadow: var(--shadow-glow-primary);
}

.btn-diagnostic.secondary {
    background: var(--bg-glass);
    color: white;
    border: 1px solid var(--bg-glass-border);
}

.btn-diagnostic.success {
    background: var(--status-healthy-gradient);
    color: white;
}

.btn-diagnostic.icon-only {
    padding: var(--spacing-sm);
    width: 40px;
    height: 40px;
    justify-content: center;
}

.btn-diagnostic i {
    font-size: 1rem;
}

.btn-diagnostic.loading i {
    animation: spin 1s linear infinite;
}

/* ============================================
   Copy Button
   ============================================ */

.copy-button {
    display: inline-flex;
    align-items: center;
    gap: var(--spacing-xs);
    padding: var(--spacing-xs) var(--spacing-sm);
    background: transparent;
    border: 1px solid rgba(255, 255, 255, 0.1);
    border-radius: var(--radius-sm);
    color: #9ca3af;
    font-size: 0.75rem;
    cursor: pointer;
    transition: all var(--transition-fast);
}

.copy-button:hover {
    background: rgba(255, 255, 255, 0.05);
    color: white;
    border-color: rgba(255, 255, 255, 0.2);
}

.copy-button.copied {
    background: rgba(16, 185, 129, 0.1);
    color: var(--accent-success);
    border-color: var(--accent-success);
}

.copy-button i {
    font-size: 0.875rem;
}

/* ============================================
   Info Row (for key-value pairs)
   ============================================ */

.info-row {
    display: flex;
    justify-content: space-between;
    align-items: center;
    padding: var(--spacing-sm) 0;
    border-bottom: 1px solid rgba(255, 255, 255, 0.05);
}

.info-row:last-child {
    border-bottom: none;
}

.info-label {
    font-size: 0.875rem;
    color: #9ca3af;
    font-weight: 500;
}

.info-value {
    font-size: 0.875rem;
    color: white;
    font-weight: 600;
    display: flex;
    align-items: center;
    gap: var(--spacing-sm);
}

/* ============================================
   Responsive Design
   ============================================ */

@media (max-width: 768px) {
    .diagnostics-header {
        flex-direction: column;
        align-items: flex-start;
        gap: var(--spacing-md);
    }
    
    .diagnostics-actions {
        width: 100%;
        justify-content: space-between;
    }
    
    .metric-value {
        font-size: 1.5rem;
    }
    
    .metric-icon {
        width: 40px;
        height: 40px;
        font-size: 1.25rem;
    }
}

/* ============================================
   Utility Classes
   ============================================ */

.fade-in {
    animation: fadeIn 0.5s ease-out;
}

.fade-in-scale {
    animation: fadeInScale 0.5s ease-out;
}

.slide-in-right {
    animation: slideInRight 0.5s ease-out;
}

.text-gradient-primary {
    background: linear-gradient(135deg, var(--accent-primary), var(--accent-secondary));
    -webkit-background-clip: text;
    -webkit-text-fill-color: transparent;
    background-clip: text;
}

.text-success {
    color: var(--accent-success);
}

.text-warning {
    color: var(--accent-warning);
}

.text-danger {
    color: var(--accent-danger);
}

.text-muted {
    color: #9ca3af;
}
```

**Action**: Create this file with all the CSS design system.

---

### **Task 2: Create Shared Components** 🧩

#### 2.1 StatusBadge Component
**File**: `Components/Shared/Diagnostics/StatusBadge.razor`

```razor
@* Status badge with animated dot indicator *@
<span class="status-badge @StatusClass" title="@Tooltip">
    <span class="status-dot"></span>
    @StatusText
</span>

@code {
    [Parameter] public HealthStatus Status { get; set; } = HealthStatus.Unknown;
    [Parameter] public string? CustomText { get; set; }
    [Parameter] public string? CustomTooltip { get; set; }
    
    private string StatusClass => Status switch
    {
        HealthStatus.Healthy => "healthy",
        HealthStatus.Warning => "warning",
        HealthStatus.Error => "error",
        HealthStatus.Critical => "critical",
        _ => "unknown"
    };
    
    private string StatusText => CustomText ?? Status switch
    {
        HealthStatus.Healthy => "Healthy",
        HealthStatus.Warning => "Warning",
        HealthStatus.Error => "Error",
        HealthStatus.Critical => "Critical",
        _ => "Unknown"
    };
    
    private string Tooltip => CustomTooltip ?? Status switch
    {
        HealthStatus.Healthy => "System is operating normally",
        HealthStatus.Warning => "Minor issues detected, monitoring recommended",
        HealthStatus.Error => "Errors detected, attention needed",
        HealthStatus.Critical => "Critical issues detected, immediate action required",
        _ => "Status unknown or not checked"
    };
}
```

#### 2.2 MetricCard Component
**File**: `Components/Shared/Diagnostics/MetricCard.razor`

```razor
@* Reusable metric display card with animations *@
<div class="metric-card @SizeClass @(Clickable ? "clickable" : "")" @onclick="HandleClick">
    <div class="metric-card-header">
        <div class="metric-icon @IconColorClass">
            <i class="@IconClass"></i>
        </div>
        @if (ShowStatus)
        {
            <StatusBadge Status="@Status" CustomText="@StatusText" />
        }
    </div>
    
    <div class="metric-content">
        <div class="metric-label">@Label</div>
        
        @if (IsLoading)
        {
            <div class="skeleton-loader skeleton-text large"></div>
            @if (!string.IsNullOrEmpty(SubText))
            {
                <div class="skeleton-loader skeleton-text small" style="width: 60%;"></div>
            }
        }
        else
        {
            <div class="metric-value @(Animated ? "animated" : "")">
                @if (NumericValue.HasValue)
                {
                    <span>@NumericValue.Value.ToString(ValueFormat)</span>
                    @if (!string.IsNullOrEmpty(Unit))
                    {
                        <span class="metric-unit">@Unit</span>
                    }
                }
                else
                {
                    <span>@TextValue</span>
                }
            </div>
            
            @if (!string.IsNullOrEmpty(SubText))
            {
                <div class="metric-subtext @SubTextClass">
                    @if (!string.IsNullOrEmpty(SubTextIcon))
                    {
                        <i class="@SubTextIcon"></i>
                    }
                    @SubText
                </div>
            }
        }
    </div>
    
    @if (ChildContent != null)
    {
        <div class="metric-card-footer">
            @ChildContent
        </div>
    }
</div>

@code {
    [Parameter] public string Label { get; set; } = "";
    [Parameter] public string IconClass { get; set; } = "bi bi-info-circle";
    [Parameter] public string IconColorClass { get; set; } = "primary";
    [Parameter] public double? NumericValue { get; set; }
    [Parameter] public string? TextValue { get; set; }
    [Parameter] public string ValueFormat { get; set; } = "N0";
    [Parameter] public string Unit { get; set; } = "";
    [Parameter] public string? SubText { get; set; }
    [Parameter] public string? SubTextIcon { get; set; }
    [Parameter] public string SubTextClass { get; set; } = "";
    [Parameter] public HealthStatus Status { get; set; } = HealthStatus.Unknown;
    [Parameter] public string? StatusText { get; set; }
    [Parameter] public bool ShowStatus { get; set; } = true;
    [Parameter] public bool IsLoading { get; set; }
    [Parameter] public bool Animated { get; set; } = true;
    [Parameter] public string Size { get; set; } = "medium"; // small, medium, large
    [Parameter] public bool Clickable { get; set; }
    [Parameter] public EventCallback OnClick { get; set; }
    [Parameter] public RenderFragment? ChildContent { get; set; }
    
    private string SizeClass => $"size-{Size}";
    
    private async Task HandleClick()
    {
        if (Clickable && OnClick.HasDelegate)
        {
            await OnClick.InvokeAsync();
        }
    }
}
```

#### 2.3 CopyButton Component
**File**: `Components/Shared/Diagnostics/CopyButton.razor`

```razor
@inject IJSRuntime JS

@* Click-to-copy button with feedback *@
<button class="copy-button @(copied ? "copied" : "")" @onclick="CopyToClipboard" title="@(copied ? "Copied!" : "Click to copy")">
    <i class="@IconClass"></i>
    @if (ShowText)
    {
        <span>@ButtonText</span>
    }
</button>

@code {
    [Parameter] public string TextToCopy { get; set; } = "";
    [Parameter] public bool ShowText { get; set; } = true;
    
    private bool copied = false;
    private string ButtonText => copied ? "Copied!" : "Copy";
    private string IconClass => copied ? "bi bi-check-lg" : "bi bi-clipboard";
    
    private async Task CopyToClipboard()
    {
        try
        {
            await JS.InvokeVoidAsync("navigator.clipboard.writeText", TextToCopy);
            copied = true;
            StateHasChanged();
            
            // Reset after 2 seconds
            await Task.Delay(2000);
            copied = false;
            StateHasChanged();
        }
        catch (Exception)
        {
            // Clipboard API not available or permission denied
        }
    }
}
```

#### 2.4 SkeletonLoader Component
**File**: `Components/Shared/Diagnostics/SkeletonLoader.razor`

```razor
@* Animated skeleton loader for loading states *@
<div class="skeleton-loader @TypeClass" style="@Style"></div>

@code {
    [Parameter] public string Type { get; set; } = "text"; // text, circle, rectangle
    [Parameter] public string Size { get; set; } = "medium"; // small, medium, large
    [Parameter] public string? Width { get; set; }
    [Parameter] public string? Height { get; set; }
    
    private string TypeClass => Type switch
    {
        "circle" => "skeleton-circle",
        "text" => $"skeleton-text {Size}",
        _ => ""
    };
    
    private string Style
    {
        get
        {
            var styles = new List<string>();
            if (!string.IsNullOrEmpty(Width))
                styles.Add($"width: {Width}");
            if (!string.IsNullOrEmpty(Height))
                styles.Add($"height: {Height}");
            return string.Join("; ", styles);
        }
    }
}
```

---

### **Task 3: Update Models** 📊

#### 3.1 Create HealthStatus Enum
**File**: `Models/Diagnostics/HealthStatus.cs`

```csharp
namespace GFC.BlazorServer.Models.Diagnostics;

/// <summary>
/// Represents the health status of a system component
/// </summary>
public enum HealthStatus
{
    /// <summary>
    /// Status is unknown or not yet checked
    /// </summary>
    Unknown = 0,
    
    /// <summary>
    /// Component is operating normally
    /// </summary>
    Healthy = 1,
    
    /// <summary>
    /// Minor issues detected, monitoring recommended
    /// </summary>
    Warning = 2,
    
    /// <summary>
    /// Errors detected, attention needed
    /// </summary>
    Error = 3,
    
    /// <summary>
    /// Critical issues detected, immediate action required
    /// </summary>
    Critical = 4
}
```

#### 3.2 Expand SystemDiagnosticsInfo Model
**File**: `Models/SystemDiagnosticsInfo.cs` (UPDATE EXISTING)

```csharp
using GFC.BlazorServer.Models.Diagnostics;

namespace GFC.BlazorServer.Models;

public class SystemDiagnosticsInfo
{
    // Existing properties
    public string EnvironmentName { get; set; } = string.Empty;
    public string ApplicationVersion { get; set; } = string.Empty;
    public string DatabaseProvider { get; set; } = string.Empty;
    public string DatabaseServer { get; set; } = string.Empty;
    public string DatabaseName { get; set; } = string.Empty;
    public string GfcConnectionStringName { get; set; } = "GFC";
    public string AgentApiBaseUrl { get; set; } = string.Empty;
    public bool? AgentApiReachable { get; set; }
    public string? AgentApiError { get; set; }
    
    // NEW: Timestamp and health status
    public DateTime CollectedAt { get; set; } = DateTime.UtcNow;
    public HealthStatus OverallHealth { get; set; } = HealthStatus.Unknown;
    public HealthStatus DatabaseHealth { get; set; } = HealthStatus.Unknown;
    public HealthStatus AgentApiHealth { get; set; } = HealthStatus.Unknown;
    
    // NEW: Performance metrics (basic for Phase 1)
    public long MemoryUsedBytes { get; set; }
    public TimeSpan Uptime { get; set; }
    public int ErrorCount24Hours { get; set; }
    
    // Helper properties
    public double MemoryUsedMB => MemoryUsedBytes / 1024.0 / 1024.0;
    public string UptimeFormatted => FormatUptime(Uptime);
    
    private static string FormatUptime(TimeSpan uptime)
    {
        if (uptime.TotalDays >= 1)
            return $"{(int)uptime.TotalDays}d {uptime.Hours}h";
        if (uptime.TotalHours >= 1)
            return $"{(int)uptime.TotalHours}h {uptime.Minutes}m";
        return $"{(int)uptime.TotalMinutes}m";
    }
}
```

---

### **Task 4: Update DiagnosticsService** ⚙️

#### 4.1 Enhance DiagnosticsService
**File**: `Services/DiagnosticsService.cs` (UPDATE EXISTING)

Add these new methods and update existing ones:

```csharp
// Add these using statements at the top
using System.Diagnostics;
using GFC.BlazorServer.Models.Diagnostics;

// Add this field to track application start time
private static readonly DateTime _applicationStartTime = DateTime.UtcNow;

// UPDATE the GetDiagnosticsAsync method
public async Task<SystemDiagnosticsInfo> GetDiagnosticsAsync(CancellationToken cancellationToken = default)
{
    var diagnostics = new SystemDiagnosticsInfo
    {
        EnvironmentName = _environment.EnvironmentName,
        ApplicationVersion = _versionService.GetFullVersion(),
        AgentApiBaseUrl = _configuration["AgentApi:BaseUrl"] ?? string.Empty,
        CollectedAt = DateTime.UtcNow,
        Uptime = DateTime.UtcNow - _applicationStartTime
    };

    // Collect performance metrics
    CollectPerformanceMetrics(diagnostics);

    // Populate database info
    await PopulateDatabaseInfoAsync(diagnostics, cancellationToken);
    
    // Populate agent status
    await PopulateAgentStatusAsync(diagnostics, cancellationToken);
    
    // Calculate overall health
    CalculateOverallHealth(diagnostics);

    return diagnostics;
}

// NEW: Collect basic performance metrics
private void CollectPerformanceMetrics(SystemDiagnosticsInfo diagnostics)
{
    try
    {
        var process = Process.GetCurrentProcess();
        diagnostics.MemoryUsedBytes = process.WorkingSet64;
    }
    catch
    {
        diagnostics.MemoryUsedBytes = 0;
    }
    
    // TODO: In Phase 2, we'll add error count from logs/database
    diagnostics.ErrorCount24Hours = 0;
}

// UPDATE: PopulateDatabaseInfoAsync to set health status
private async Task PopulateDatabaseInfoAsync(SystemDiagnosticsInfo diagnostics, CancellationToken cancellationToken)
{
    var connection = _dbContext.Database.GetDbConnection();
    diagnostics.DatabaseProvider = connection.GetType().Name;

    var shouldCloseConnection = false;
    try
    {
        if (connection.State == ConnectionState.Closed)
        {
            await connection.OpenAsync(cancellationToken);
            shouldCloseConnection = true;
        }

        using var command = connection.CreateCommand();
        command.CommandText = "SELECT @@SERVERNAME, DB_NAME()";

        using var reader = await command.ExecuteReaderAsync(cancellationToken);
        if (await reader.ReadAsync(cancellationToken))
        {
            diagnostics.DatabaseServer = reader.IsDBNull(0) ? "(unknown)" : reader.GetString(0);
            diagnostics.DatabaseName = reader.IsDBNull(1) ? "(unknown)" : reader.GetString(1);
        }
        
        // Database is reachable and working
        diagnostics.DatabaseHealth = HealthStatus.Healthy;
    }
    catch (Exception)
    {
        diagnostics.DatabaseHealth = HealthStatus.Error;
        diagnostics.DatabaseServer = "(error)";
        diagnostics.DatabaseName = "(error)";
    }
    finally
    {
        if (shouldCloseConnection && connection.State == ConnectionState.Open)
        {
            await connection.CloseAsync();
        }
    }
}

// UPDATE: PopulateAgentStatusAsync to set health status
private async Task PopulateAgentStatusAsync(SystemDiagnosticsInfo diagnostics, CancellationToken cancellationToken)
{
    if (string.IsNullOrWhiteSpace(diagnostics.AgentApiBaseUrl))
    {
        diagnostics.AgentApiReachable = null;
        diagnostics.AgentApiError = "Agent API base URL not configured.";
        diagnostics.AgentApiHealth = HealthStatus.Unknown;
        return;
    }

    try
    {
        using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        cts.CancelAfter(TimeSpan.FromSeconds(3));
        var reachable = await _controllerClient.PingAsync(cts.Token);
        diagnostics.AgentApiReachable = reachable;
        
        if (reachable)
        {
            diagnostics.AgentApiHealth = HealthStatus.Healthy;
        }
        else
        {
            diagnostics.AgentApiError = "Agent API responded but returned a non-success status.";
            diagnostics.AgentApiHealth = HealthStatus.Error;
        }
    }
    catch (Exception ex)
    {
        diagnostics.AgentApiReachable = false;
        diagnostics.AgentApiError = ex.Message;
        diagnostics.AgentApiHealth = HealthStatus.Error;
    }
}

// NEW: Calculate overall health based on component health
private void CalculateOverallHealth(SystemDiagnosticsInfo diagnostics)
{
    var healthStatuses = new[]
    {
        diagnostics.DatabaseHealth,
        diagnostics.AgentApiHealth
    };
    
    // Overall health is the worst status among components
    if (healthStatuses.Any(h => h == HealthStatus.Critical))
        diagnostics.OverallHealth = HealthStatus.Critical;
    else if (healthStatuses.Any(h => h == HealthStatus.Error))
        diagnostics.OverallHealth = HealthStatus.Error;
    else if (healthStatuses.Any(h => h == HealthStatus.Warning))
        diagnostics.OverallHealth = HealthStatus.Warning;
    else if (healthStatuses.All(h => h == HealthStatus.Healthy))
        diagnostics.OverallHealth = HealthStatus.Healthy;
    else
        diagnostics.OverallHealth = HealthStatus.Unknown;
}
```

---

### **Task 5: Modernize SystemDiagnostics.razor** 🎨

#### 5.1 Complete Page Redesign
**File**: `Components/Pages/SystemDiagnostics.razor` (REPLACE ENTIRE FILE)

```razor
@page "/admin/system-diagnostics"
@attribute [Authorize(Policy = AppPolicies.RequireAdmin)]

@using GFC.BlazorServer.Models
@using GFC.BlazorServer.Models.Diagnostics
@inject DiagnosticsService DiagnosticsService

<PageTitle>System Diagnostics - GFC Studio</PageTitle>

<div class="diagnostics-page">
    <!-- Header with title and actions -->
    <div class="diagnostics-header">
        <h1 class="diagnostics-title">
            <i class="bi bi-activity me-2"></i>
            System Diagnostics
        </h1>
        
        <div class="diagnostics-actions">
            <div class="last-updated @(_isRefreshing ? "refreshing" : "")">
                <i class="bi bi-@(_isRefreshing ? "arrow-repeat" : "clock")"></i>
                @if (_info != null)
                {
                    <span>@GetRelativeTime(_info.CollectedAt)</span>
                }
            </div>
            
            <button class="btn-diagnostic secondary icon-only @(_isRefreshing ? "loading" : "")" 
                    @onclick="RefreshDiagnostics" 
                    disabled="@_isRefreshing"
                    title="Refresh diagnostics">
                <i class="bi bi-arrow-clockwise"></i>
            </button>
        </div>
    </div>

    @if (_info is null)
    {
        <!-- Loading State with Skeleton Loaders -->
        <div class="row g-4">
            <div class="col-md-4">
                <div class="metric-card">
                    <div class="metric-card-header">
                        <SkeletonLoader Type="circle" />
                    </div>
                    <div class="metric-content">
                        <SkeletonLoader Type="text" Size="small" Width="40%" />
                        <SkeletonLoader Type="text" Size="large" />
                        <SkeletonLoader Type="text" Size="small" Width="60%" />
                    </div>
                </div>
            </div>
            <div class="col-md-4">
                <div class="metric-card">
                    <div class="metric-card-header">
                        <SkeletonLoader Type="circle" />
                    </div>
                    <div class="metric-content">
                        <SkeletonLoader Type="text" Size="small" Width="40%" />
                        <SkeletonLoader Type="text" Size="large" />
                        <SkeletonLoader Type="text" Size="small" Width="60%" />
                    </div>
                </div>
            </div>
            <div class="col-md-4">
                <div class="metric-card">
                    <div class="metric-card-header">
                        <SkeletonLoader Type="circle" />
                    </div>
                    <div class="metric-content">
                        <SkeletonLoader Type="text" Size="small" Width="40%" />
                        <SkeletonLoader Type="text" Size="large" />
                        <SkeletonLoader Type="text" Size="small" Width="60%" />
                    </div>
                </div>
            </div>
        </div>
    }
    else
    {
        <!-- Overall Health Summary -->
        <div class="row g-4 mb-4">
            <div class="col-12">
                <MetricCard 
                    Label="Overall System Health"
                    IconClass="bi bi-heart-pulse"
                    IconColorClass="@GetHealthIconColor(_info.OverallHealth)"
                    Status="@_info.OverallHealth"
                    TextValue="@_info.OverallHealth.ToString()"
                    SubText="@GetHealthDescription(_info.OverallHealth)"
                    SubTextIcon="bi bi-info-circle"
                    Size="large"
                    IsLoading="@_isRefreshing" />
            </div>
        </div>

        <!-- Quick Stats Row -->
        <div class="row g-4 mb-4">
            <div class="col-md-4">
                <MetricCard 
                    Label="System Uptime"
                    IconClass="bi bi-clock-history"
                    IconColorClass="info"
                    NumericValue="null"
                    TextValue="@_info.UptimeFormatted"
                    SubText="@($"Since {(_info.CollectedAt - _info.Uptime):MMM dd, HH:mm}")"
                    Status="HealthStatus.Healthy"
                    ShowStatus="false"
                    IsLoading="@_isRefreshing" />
            </div>
            
            <div class="col-md-4">
                <MetricCard 
                    Label="Memory Usage"
                    IconClass="bi bi-memory"
                    IconColorClass="primary"
                    NumericValue="@_info.MemoryUsedMB"
                    ValueFormat="N1"
                    Unit="MB"
                    SubText="Working set memory"
                    Status="HealthStatus.Healthy"
                    ShowStatus="false"
                    IsLoading="@_isRefreshing" />
            </div>
            
            <div class="col-md-4">
                <MetricCard 
                    Label="Errors (24h)"
                    IconClass="bi bi-exclamation-triangle"
                    IconColorClass="@(_info.ErrorCount24Hours > 0 ? "warning" : "success")"
                    NumericValue="@_info.ErrorCount24Hours"
                    SubText="@(_info.ErrorCount24Hours == 0 ? "No errors detected" : "Errors logged")"
                    SubTextClass="@(_info.ErrorCount24Hours > 0 ? "negative" : "positive")"
                    Status="@(_info.ErrorCount24Hours > 0 ? HealthStatus.Warning : HealthStatus.Healthy)"
                    ShowStatus="false"
                    IsLoading="@_isRefreshing" />
            </div>
        </div>

        <!-- Detailed Diagnostics -->
        <div class="row g-4">
            <!-- Application Info -->
            <div class="col-md-4">
                <div class="metric-card">
                    <div class="metric-card-header">
                        <div class="metric-icon success">
                            <i class="bi bi-app-indicator"></i>
                        </div>
                        <StatusBadge Status="HealthStatus.Healthy" CustomText="Running" />
                    </div>
                    <div class="metric-content">
                        <div class="metric-label">Application</div>
                        
                        <div class="info-row">
                            <span class="info-label">Environment</span>
                            <span class="info-value">
                                @_info.EnvironmentName
                                @if (_info.EnvironmentName == "Production")
                                {
                                    <i class="bi bi-shield-check text-success"></i>
                                }
                                else
                                {
                                    <i class="bi bi-code-square text-warning"></i>
                                }
                            </span>
                        </div>
                        
                        <div class="info-row">
                            <span class="info-label">Version</span>
                            <span class="info-value">
                                @_info.ApplicationVersion
                                <CopyButton TextToCopy="@_info.ApplicationVersion" ShowText="false" />
                            </span>
                        </div>
                    </div>
                </div>
            </div>

            <!-- Database Info -->
            <div class="col-md-4">
                <div class="metric-card">
                    <div class="metric-card-header">
                        <div class="metric-icon @GetHealthIconColor(_info.DatabaseHealth)">
                            <i class="bi bi-database"></i>
                        </div>
                        <StatusBadge Status="@_info.DatabaseHealth" />
                    </div>
                    <div class="metric-content">
                        <div class="metric-label">Database</div>
                        
                        <div class="info-row">
                            <span class="info-label">Provider</span>
                            <span class="info-value">@_info.DatabaseProvider</span>
                        </div>
                        
                        <div class="info-row">
                            <span class="info-label">Server</span>
                            <span class="info-value">
                                @_info.DatabaseServer
                                <CopyButton TextToCopy="@_info.DatabaseServer" ShowText="false" />
                            </span>
                        </div>
                        
                        <div class="info-row">
                            <span class="info-label">Database</span>
                            <span class="info-value">
                                @_info.DatabaseName
                                <CopyButton TextToCopy="@_info.DatabaseName" ShowText="false" />
                            </span>
                        </div>
                        
                        <div class="info-row">
                            <span class="info-label">Connection</span>
                            <span class="info-value">@_info.GfcConnectionStringName</span>
                        </div>
                    </div>
                </div>
            </div>

            <!-- Agent API Info -->
            <div class="col-md-4">
                <div class="metric-card">
                    <div class="metric-card-header">
                        <div class="metric-icon @GetHealthIconColor(_info.AgentApiHealth)">
                            <i class="bi bi-hdd-network"></i>
                        </div>
                        <StatusBadge Status="@_info.AgentApiHealth" />
                    </div>
                    <div class="metric-content">
                        <div class="metric-label">Agent API</div>
                        
                        <div class="info-row">
                            <span class="info-label">Base URL</span>
                            <span class="info-value">
                                @if (string.IsNullOrEmpty(_info.AgentApiBaseUrl))
                                {
                                    <span class="text-muted">Not configured</span>
                                }
                                else
                                {
                                    <span>@_info.AgentApiBaseUrl</span>
                                    <CopyButton TextToCopy="@_info.AgentApiBaseUrl" ShowText="false" />
                                }
                            </span>
                        </div>
                        
                        <div class="info-row">
                            <span class="info-label">Reachability</span>
                            <span class="info-value">
                                @if (_info.AgentApiReachable is null)
                                {
                                    <span class="text-muted">Not checked</span>
                                }
                                else if (_info.AgentApiReachable == true)
                                {
                                    <span class="text-success">
                                        <i class="bi bi-check-circle-fill"></i> OK
                                    </span>
                                }
                                else
                                {
                                    <span class="text-danger">
                                        <i class="bi bi-x-circle-fill"></i> Failed
                                    </span>
                                }
                            </span>
                        </div>
                        
                        @if (!string.IsNullOrWhiteSpace(_info.AgentApiError))
                        {
                            <div class="info-row">
                                <span class="info-label">Error</span>
                                <span class="info-value text-danger" style="font-size: 0.75rem;">
                                    @_info.AgentApiError
                                </span>
                            </div>
                        }
                    </div>
                </div>
            </div>
        </div>
    }
</div>

@code
{
    private SystemDiagnosticsInfo? _info;
    private bool _isRefreshing = false;
    private System.Threading.Timer? _autoRefreshTimer;

    protected override async Task OnInitializedAsync()
    {
        await LoadDiagnostics();
        
        // Setup auto-refresh every 30 seconds
        _autoRefreshTimer = new System.Threading.Timer(async _ =>
        {
            await InvokeAsync(async () =>
            {
                await RefreshDiagnostics();
            });
        }, null, TimeSpan.FromSeconds(30), TimeSpan.FromSeconds(30));
    }

    private async Task LoadDiagnostics()
    {
        _info = await DiagnosticsService.GetDiagnosticsAsync();
    }

    private async Task RefreshDiagnostics()
    {
        _isRefreshing = true;
        StateHasChanged();
        
        await Task.Delay(300); // Small delay for visual feedback
        await LoadDiagnostics();
        
        _isRefreshing = false;
        StateHasChanged();
    }

    private string GetRelativeTime(DateTime timestamp)
    {
        var diff = DateTime.UtcNow - timestamp;
        
        if (diff.TotalSeconds < 60)
            return "Just now";
        if (diff.TotalMinutes < 60)
            return $"{(int)diff.TotalMinutes}m ago";
        if (diff.TotalHours < 24)
            return $"{(int)diff.TotalHours}h ago";
        return timestamp.ToLocalTime().ToString("MMM dd, HH:mm");
    }

    private string GetHealthIconColor(HealthStatus status) => status switch
    {
        HealthStatus.Healthy => "success",
        HealthStatus.Warning => "warning",
        HealthStatus.Error => "danger",
        HealthStatus.Critical => "danger",
        _ => "info"
    };

    private string GetHealthDescription(HealthStatus status) => status switch
    {
        HealthStatus.Healthy => "All systems operational",
        HealthStatus.Warning => "Some components need attention",
        HealthStatus.Error => "Errors detected in one or more components",
        HealthStatus.Critical => "Critical issues require immediate attention",
        _ => "System status is being evaluated"
    };

    public void Dispose()
    {
        _autoRefreshTimer?.Dispose();
    }
}
```

---

### **Task 6: Update App.razor to Include CSS** 📝

**File**: `Components/App.razor` (ADD CSS REFERENCE)

Add this line in the `<head>` section:

```razor
<link rel="stylesheet" href="css/diagnostics.css" />
```

---

### **Task 7: Create _Imports for Diagnostics Components** 📦

**File**: `Components/Shared/Diagnostics/_Imports.razor` (CREATE NEW)

```razor
@using GFC.BlazorServer.Models.Diagnostics
```

---

## Testing Checklist

After implementation, verify:

- [ ] Page loads without errors
- [ ] Skeleton loaders appear during initial load
- [ ] All three cards display with correct data
- [ ] Status badges show correct colors and pulse animation
- [ ] Click-to-copy buttons work for connection strings
- [ ] Auto-refresh works every 30 seconds
- [ ] Manual refresh button works and shows loading state
- [ ] "Last Updated" timestamp updates correctly
- [ ] Hover effects work on cards
- [ ] Responsive design works on mobile
- [ ] All icons display correctly (Bootstrap Icons)
- [ ] Health status calculations are correct
- [ ] Memory usage displays in MB
- [ ] Uptime formats correctly

---

## Files to Create

1. ✅ `wwwroot/css/diagnostics.css`
2. ✅ `Components/Shared/Diagnostics/StatusBadge.razor`
3. ✅ `Components/Shared/Diagnostics/MetricCard.razor`
4. ✅ `Components/Shared/Diagnostics/CopyButton.razor`
5. ✅ `Components/Shared/Diagnostics/SkeletonLoader.razor`
6. ✅ `Components/Shared/Diagnostics/_Imports.razor`
7. ✅ `Models/Diagnostics/HealthStatus.cs`

## Files to Update

1. ✅ `Models/SystemDiagnosticsInfo.cs` - Expand with new properties
2. ✅ `Services/DiagnosticsService.cs` - Add performance metrics and health calculations
3. ✅ `Components/Pages/SystemDiagnostics.razor` - Complete redesign
4. ✅ `Components/App.razor` - Add CSS reference

---

## Success Criteria

✅ **Visual Excellence**: Page looks modern with gradients, animations, and premium feel
✅ **Smooth Animations**: Fade-ins, skeleton loaders, pulsing status badges
✅ **Real-time Updates**: Auto-refresh every 30 seconds with visual feedback
✅ **Interactive Elements**: Hover effects, click-to-copy, manual refresh
✅ **Health Monitoring**: Color-coded status badges for each component
✅ **Performance**: Page loads quickly, animations are smooth (60fps)
✅ **Responsive**: Works on desktop, tablet, and mobile

---

## Estimated Time

- **Task 1** (CSS): 30 minutes
- **Task 2** (Components): 1 hour
- **Task 3** (Models): 15 minutes
- **Task 4** (Service): 30 minutes
- **Task 5** (Page): 45 minutes
- **Task 6-7** (Config): 5 minutes
- **Testing**: 30 minutes

**Total**: ~3.5 hours

---

## Next Steps (Phase 2)

After Phase 1 is complete and tested:
- Add performance trend charts
- Implement database health checks
- Add more detailed metrics
- Create diagnostic actions (test connections, etc.)

---

**Status**: Ready for Implementation
**Priority**: High
**Dependencies**: None
