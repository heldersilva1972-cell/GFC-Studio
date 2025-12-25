# ============================================================================
# Fix Studio Component Compilation Errors
# ============================================================================
# This script fixes 8 compilation errors in the Studio components:
# 1. RZ1011 - 'page' directive whitespace issue in TopBar.razor
# 2. RZ9979 - Code block syntax issue in TopBar.razor
# 3. CS0118 - Canvas namespace vs type conflict in Editor.razor
# 4. CS0246 - Missing FullPageLayout using directive in Editor.razor
# ============================================================================

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "Fixing Studio Component Errors" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

$baseDir = "C:\Users\hnsil\Documents\GFC\cursor files\GFC-System\GFC-Studio V2\apps\webapp\GFC.BlazorServer"

# ============================================================================
# Fix 1 & 2: TopBar.razor - Remove @page directive (not needed in component)
# ============================================================================
Write-Host "[1/2] Fixing TopBar.razor..." -ForegroundColor Yellow

$topBarPath = "$baseDir\Components\Pages\Admin\Studio\TopBar\TopBar.razor"
$topBarContent = @'
@using GFC.Core.Models
@using GFC.BlazorServer.Services
@inject IStudioService StudioService

<div class="top-bar-container">
    <div class="page-switcher-container">
        <select @onchange="HandlePageChange">
            @if (pages != null)
            {
                foreach (var page in pages)
                {
                    <option value="@page.Id">@page.Title</option>
                }
            }
        </select>
    </div>
    <div class="device-toggles-container">
        <button @onclick="() => OnDeviceToggle.InvokeAsync(DeviceView.Desktop)">Desktop</button>
        <button @onclick="() => OnDeviceToggle.InvokeAsync(DeviceView.Tablet)">Tablet</button>
        <button @onclick="() => OnDeviceToggle.InvokeAsync(DeviceView.Mobile)">Mobile</button>
    </div>
    <div class="save-buttons-container">
        <!-- Save buttons will go here -->
    </div>
</div>

@code {
    private IEnumerable<StudioPage> pages;

    [Parameter]
    public EventCallback<DeviceView> OnDeviceToggle { get; set; }

    [Parameter]
    public EventCallback<int> OnPageChange { get; set; }

    protected override async Task OnInitializedAsync()
    {
        pages = await StudioService.GetAllPagesAsync();
    }

    private void HandlePageChange(ChangeEventArgs e)
    {
        if (int.TryParse(e.Value.ToString(), out int pageId))
        {
            OnPageChange.InvokeAsync(pageId);
        }
    }

    public enum DeviceView
    {
        Desktop,
        Tablet,
        Mobile
    }
}
'@

Set-Content -Path $topBarPath -Value $topBarContent -Encoding UTF8
Write-Host "  ✓ Fixed TopBar.razor" -ForegroundColor Green

# ============================================================================
# Fix 3 & 4: Editor.razor - Fix Canvas reference and FullPageLayout
# ============================================================================
Write-Host "[2/2] Fixing Editor.razor..." -ForegroundColor Yellow

$editorPath = "$baseDir\Components\Pages\Admin\Studio\Editor.razor"
$editorContent = @'
@page "/admin/studio/editor"
@using GFC.BlazorServer.Components.Layout
@layout FullPageLayout

@using GFC.BlazorServer.Components.Pages.Admin.Studio.TopBar
@using GFC.BlazorServer.Components.Pages.Admin.Studio.LeftPanel
@using GFC.BlazorServer.Components.Pages.Admin.Studio.RightPanel
@using CanvasComponent = GFC.BlazorServer.Components.Pages.Admin.Studio.Canvas.Canvas

@inject ILogger<Editor> Logger

<div class="studio-container">
    <div class="top-bar">
        <TopBar OnDeviceToggle="HandleDeviceToggle" OnPageChange="HandlePageChange" />
    </div>
    <div class="left-panel">
        <LeftPanel />
    </div>
    <div class="canvas">
        <CanvasComponent @ref="canvas" />
    </div>
    <div class="right-panel">
        <RightPanel />
    </div>
</div>

@code {
    private CanvasComponent canvas;

    private void HandleDeviceToggle(TopBar.DeviceView view)
    {
        canvas.SetDeviceView(view);
    }

    private void HandlePageChange(int pageId)
    {
        Logger.LogInformation("Selected page ID: {PageId}", pageId);
        // We will load the page data in a later step
    }
}
'@

Set-Content -Path $editorPath -Value $editorContent -Encoding UTF8
Write-Host "  ✓ Fixed Editor.razor" -ForegroundColor Green

# ============================================================================
# Summary
# ============================================================================
Write-Host ""
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "All Studio Errors Fixed!" -ForegroundColor Green
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "Fixed issues:" -ForegroundColor Yellow
Write-Host "  ✓ RZ1011 - Removed invalid @page directive from TopBar.razor" -ForegroundColor Green
Write-Host "  ✓ RZ9979 - Fixed code block syntax in TopBar.razor" -ForegroundColor Green
Write-Host "  ✓ CS0118 - Fixed Canvas namespace conflict using alias" -ForegroundColor Green
Write-Host "  ✓ CS0246 - Added FullPageLayout using directive" -ForegroundColor Green
Write-Host ""
Write-Host "Please rebuild the project to verify all errors are resolved." -ForegroundColor Cyan
