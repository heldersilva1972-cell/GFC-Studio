# System Diagnostics Modernization Plan

## Overview
Transform the System Diagnostics page from a basic information display into a comprehensive, real-time system monitoring and diagnostic dashboard with modern UI, animations, and extensive diagnostic capabilities.

---

## Design Philosophy

### Visual Excellence
- **Modern Aesthetics**: Glassmorphism, gradients, vibrant colors, dark mode support
- **Smooth Animations**: Fade-ins, skeleton loaders, animated counters, pulsing status indicators
- **Interactive Elements**: Hover effects, expandable cards, click-to-copy, tooltips
- **Responsive Design**: Mobile-friendly, adaptive layouts
- **Premium Feel**: State-of-the-art design that wows users

### User Experience
- **Real-time Updates**: Auto-refresh with visual indicators
- **Actionable Insights**: Not just data, but recommendations and actions
- **Progressive Disclosure**: Summary view with drill-down capabilities
- **Search & Filter**: Quick access to specific diagnostics
- **Export Capabilities**: Download reports for troubleshooting

---

## Architecture

### Component Structure
```
SystemDiagnostics.razor (Main Page)
├── DiagnosticsDashboard.razor (Overview)
├── DiagnosticsHeader.razor (Title, Actions, Last Updated)
├── DiagnosticsSummary.razor (Key Metrics Cards)
├── DiagnosticsCategories/ (Tabbed Sections)
│   ├── SystemPerformance.razor
│   ├── DatabaseHealth.razor
│   ├── HardwareController.razor
│   ├── CameraSystem.razor
│   ├── NetworkStatus.razor
│   ├── SecurityAudit.razor
│   ├── BackgroundJobs.razor
│   ├── IntegrationStatus.razor
│   └── ConfigurationValidation.razor
├── DiagnosticActions.razor (Test Buttons, Export)
├── DiagnosticAlerts.razor (Active Warnings/Errors)
├── DiagnosticCharts/ (Performance Trends)
│   ├── PerformanceChart.razor
│   ├── ErrorTrendChart.razor
│   └── ActivityChart.razor
└── Shared/
    ├── StatusBadge.razor (Animated status indicators)
    ├── MetricCard.razor (Reusable metric display)
    ├── HealthIndicator.razor (Visual health status)
    └── DiagnosticTimeline.razor (Event timeline)
```

### Service Layer
```
Services/Diagnostics/
├── DiagnosticsService.cs (Main orchestrator)
├── SystemPerformanceService.cs
├── DatabaseHealthService.cs
├── ControllerDiagnosticsService.cs
├── CameraDiagnosticsService.cs
├── NetworkDiagnosticsService.cs
├── SecurityDiagnosticsService.cs
├── BackgroundJobMonitorService.cs
├── IntegrationHealthService.cs
├── ConfigurationValidationService.cs
├── DiagnosticHistoryService.cs (Store historical data)
└── DiagnosticAlertService.cs (Alert management)
```

### Models
```
Models/Diagnostics/
├── SystemDiagnosticsInfo.cs (Expanded model)
├── PerformanceMetrics.cs
├── DatabaseHealthInfo.cs
├── ControllerStatusInfo.cs
├── CameraSystemInfo.cs
├── NetworkStatusInfo.cs
├── SecurityAuditInfo.cs
├── BackgroundJobInfo.cs
├── IntegrationStatusInfo.cs
├── ConfigurationValidationInfo.cs
├── DiagnosticAlert.cs
├── DiagnosticAction.cs
└── HealthStatus.cs (Enum: Healthy, Warning, Error, Unknown)
```

---

## Implementation Phases

### **Phase 1: Foundation & Core Modernization** ⭐ START HERE

#### Goals
- Modernize existing diagnostics display
- Add visual animations and modern UI
- Implement real-time updates
- Create reusable component library

#### Tasks

**1.1 Create Design System**
- [ ] Define color palette (success, warning, error, info gradients)
- [ ] Create CSS variables for theming
- [ ] Design animation keyframes (pulse, fade, slide, shimmer)
- [ ] Create typography scale
- [ ] Define spacing and layout grid

**1.2 Build Shared Components**
- [ ] `StatusBadge.razor` - Animated status indicator with pulse effect
- [ ] `MetricCard.razor` - Reusable card with hover effects, gradients
- [ ] `HealthIndicator.razor` - Visual health meter with animation
- [ ] `SkeletonLoader.razor` - Loading placeholder with shimmer effect
- [ ] `AnimatedCounter.razor` - Number counter with count-up animation
- [ ] `CopyButton.razor` - Click-to-copy with feedback animation

**1.3 Modernize Existing Diagnostics**
- [ ] Update `SystemDiagnostics.razor` with new layout
- [ ] Replace static cards with `MetricCard` components
- [ ] Add skeleton loaders during data fetch
- [ ] Implement fade-in animations when data loads
- [ ] Add status badges with pulse animations
- [ ] Add click-to-copy for connection strings, URLs
- [ ] Add icons for each section (database, server, API)

**1.4 Implement Real-time Updates**
- [ ] Add auto-refresh timer (configurable interval)
- [ ] Show "Last Updated" timestamp with relative time
- [ ] Add manual refresh button with loading animation
- [ ] Implement SignalR for push updates (optional)
- [ ] Add visual indicator when data is refreshing

**1.5 Expand SystemDiagnosticsInfo Model**
- [ ] Add timestamp fields
- [ ] Add health status enums
- [ ] Add performance metrics (memory, CPU, uptime)
- [ ] Add error counts and recent errors

**1.6 Testing & Polish**
- [ ] Test on different screen sizes
- [ ] Verify animations are smooth
- [ ] Test auto-refresh functionality
- [ ] Ensure accessibility (ARIA labels, keyboard navigation)

---

### **Phase 2: System Performance & Database Health**

#### Goals
- Add comprehensive system performance monitoring
- Implement database health diagnostics
- Create performance trend charts

#### Tasks

**2.1 System Performance Monitoring**
- [ ] Create `SystemPerformanceService.cs`
- [ ] Implement memory usage tracking (current, max, GC stats)
- [ ] Implement CPU usage monitoring
- [ ] Calculate application uptime
- [ ] Track active SignalR/Blazor circuits
- [ ] Monitor request rate (requests per minute)
- [ ] Create `PerformanceMetrics.cs` model
- [ ] Create `SystemPerformance.razor` component
- [ ] Add mini sparkline charts for trends
- [ ] Add threshold warnings (e.g., memory > 80%)

**2.2 Database Health Diagnostics**
- [ ] Create `DatabaseHealthService.cs`
- [ ] Implement connection pool monitoring
- [ ] Track query performance (average execution time)
- [ ] Get database size information
- [ ] Check last backup timestamp
- [ ] Verify migration status (current schema version)
- [ ] Test database connectivity with detailed error handling
- [ ] Create `DatabaseHealthInfo.cs` model
- [ ] Create `DatabaseHealth.razor` component
- [ ] Add "Test Connection" action button
- [ ] Add connection pool visualization

**2.3 Performance History & Trends**
- [ ] Create `DiagnosticHistoryService.cs`
- [ ] Store performance metrics in database (time-series data)
- [ ] Implement data retention policy (e.g., keep 30 days)
- [ ] Create `PerformanceChart.razor` using Chart.js or similar
- [ ] Display memory/CPU trends over 24 hours
- [ ] Display error rate trends
- [ ] Display response time trends
- [ ] Add time range selector (1h, 6h, 24h, 7d)

**2.4 Database Actions**
- [ ] Add "Test Database Connection" button
- [ ] Add "Run Health Check" button
- [ ] Add "View Slow Queries" (if logging enabled)
- [ ] Add "Check Index Fragmentation" (SQL Server)

---

### **Phase 3: Hardware Controller & Camera System**

#### Goals
- Monitor hardware controller status
- Implement camera system diagnostics
- Track device connectivity

#### Tasks

**3.1 Hardware Controller Diagnostics**
- [ ] Create `ControllerDiagnosticsService.cs`
- [ ] Check controller connection status
- [ ] Get last communication timestamp
- [ ] Retrieve firmware version (if available)
- [ ] Count connected devices (doors, readers, etc.)
- [ ] Monitor controller response time
- [ ] Create `ControllerStatusInfo.cs` model
- [ ] Create `HardwareController.razor` component
- [ ] Add "Test Controller Connection" button
- [ ] Add "Ping Controller" action
- [ ] Display device count with icons

**3.2 Camera System Diagnostics**
- [ ] Create `CameraDiagnosticsService.cs`
- [ ] Count total cameras configured
- [ ] Count active video streams
- [ ] Monitor recording storage usage
- [ ] Check NVR connection status
- [ ] Get camera discovery status
- [ ] Track failed camera connections
- [ ] Create `CameraSystemInfo.cs` model
- [ ] Create `CameraSystem.razor` component
- [ ] Add "Test All Cameras" button
- [ ] Add "Discover Cameras" action
- [ ] Display camera grid with status indicators

**3.3 Device Health Matrix**
- [ ] Create visual grid showing all devices
- [ ] Color-code by health status (green, yellow, red)
- [ ] Add hover tooltips with details
- [ ] Implement quick filtering (show only errors)

---

### **Phase 4: Network & Security**

#### Goals
- Monitor network connectivity and performance
- Implement security audit diagnostics
- Track authentication and access

#### Tasks

**4.1 Network Status Monitoring**
- [ ] Create `NetworkDiagnosticsService.cs`
- [ ] Implement ping tests to critical services
- [ ] Monitor network latency
- [ ] Track bandwidth usage (if possible)
- [ ] Verify critical ports are open/closed
- [ ] Test external API connectivity
- [ ] Create `NetworkStatusInfo.cs` model
- [ ] Create `NetworkStatus.razor` component
- [ ] Add network topology visualization
- [ ] Add "Test Network" button

**4.2 Security Audit Diagnostics**
- [ ] Create `SecurityDiagnosticsService.cs`
- [ ] Track failed login attempts (last 24h)
- [ ] Count active user sessions
- [ ] Check SSL/TLS certificate expiration
- [ ] Get last security scan timestamp
- [ ] Monitor authentication provider status
- [ ] Track password policy compliance
- [ ] Count users with 2FA enabled
- [ ] Create `SecurityAuditInfo.cs` model
- [ ] Create `SecurityAudit.razor` component
- [ ] Add security score visualization
- [ ] Add "View Security Events" button

**4.3 Integration Status**
- [ ] Create `IntegrationHealthService.cs`
- [ ] Check email server (SMTP) status
- [ ] Verify SMS gateway (if applicable)
- [ ] Test payment gateway (if applicable)
- [ ] Check cloud backup service
- [ ] Monitor external API integrations
- [ ] Create `IntegrationStatusInfo.cs` model
- [ ] Create `IntegrationStatus.razor` component
- [ ] Add dependency health matrix
- [ ] Add "Test All Integrations" button

---

### **Phase 5: Background Jobs & Configuration**

#### Goals
- Monitor scheduled tasks and background jobs
- Validate system configuration
- Implement data quality checks

#### Tasks

**5.1 Background Job Monitoring**
- [ ] Create `BackgroundJobMonitorService.cs`
- [ ] Track job queue status (pending, running, failed)
- [ ] Get last run times for each job
- [ ] Calculate next run schedule
- [ ] Store job execution history
- [ ] Track failed job details with errors
- [ ] Create `BackgroundJobInfo.cs` model
- [ ] Create `BackgroundJobs.razor` component
- [ ] Add job timeline visualization
- [ ] Add "Retry Failed Job" button
- [ ] Add "Run Job Now" button (for manual execution)

**5.2 Configuration Validation**
- [ ] Create `ConfigurationValidationService.cs`
- [ ] Verify all required settings are present
- [ ] Validate connection strings format
- [ ] Check file paths exist and are accessible
- [ ] Verify app has necessary permissions
- [ ] Check environment variables
- [ ] Validate feature flags
- [ ] Create `ConfigurationValidationInfo.cs` model
- [ ] Create `ConfigurationValidation.razor` component
- [ ] Add configuration checklist with status
- [ ] Add "Fix Configuration" suggestions

**5.3 Data Quality Metrics**
- [ ] Implement orphaned record detection
- [ ] Run data integrity checks
- [ ] Check for duplicate entries
- [ ] Display table row counts
- [ ] Add "Run Data Validation" button
- [ ] Show validation results with details

---

### **Phase 6: Alerts, Actions & Advanced Features**

#### Goals
- Implement proactive alerting system
- Add diagnostic actions and tools
- Create smart recommendations

#### Tasks

**6.1 Diagnostic Alert System**
- [ ] Create `DiagnosticAlertService.cs`
- [ ] Define alert rules and thresholds
- [ ] Implement alert detection logic
- [ ] Store alert history in database
- [ ] Create alert notification system
- [ ] Create `DiagnosticAlert.cs` model
- [ ] Create `DiagnosticAlerts.razor` component
- [ ] Display active alerts prominently
- [ ] Add alert severity levels (info, warning, critical)
- [ ] Add "Acknowledge Alert" action
- [ ] Add "Dismiss Alert" action
- [ ] Implement alert escalation for critical issues

**6.2 Diagnostic Actions**
- [ ] Create `DiagnosticActionsService.cs`
- [ ] Implement "Test Database Connection" action
- [ ] Implement "Test Agent API" action
- [ ] Implement "Test All Cameras" action
- [ ] Implement "Clear Cache" action
- [ ] Implement "Restart Services" action (if safe)
- [ ] Implement "Run Health Check" action
- [ ] Create `DiagnosticActions.razor` component
- [ ] Add action buttons with loading states
- [ ] Display action results with feedback
- [ ] Add action history log

**6.3 Export & Reporting**
- [ ] Implement "Export Diagnostics" as JSON
- [ ] Implement "Export Diagnostics" as PDF
- [ ] Create diagnostic report template
- [ ] Include all metrics, charts, and alerts
- [ ] Add timestamp and system info to report
- [ ] Add "Email Report" functionality
- [ ] Add "Schedule Reports" (daily/weekly)

**6.4 Smart Recommendations**
- [ ] Implement auto-diagnosis engine
- [ ] Create recommendation rules
- [ ] Detect common issues automatically
- [ ] Provide actionable fix suggestions
- [ ] Display recommendations prominently
- [ ] Add "Apply Fix" button (where safe)

**6.5 Search & Filter**
- [ ] Add global search for diagnostics
- [ ] Implement filter by status (healthy, warning, error)
- [ ] Implement filter by category
- [ ] Add "Show Only Issues" toggle
- [ ] Add sorting options

---

### **Phase 7: User Experience & Polish**

#### Goals
- Enhance user experience
- Add advanced visualizations
- Implement comparison and benchmarks

#### Tasks

**7.1 Advanced Visualizations**
- [ ] Create system topology diagram
- [ ] Add interactive dependency graph
- [ ] Implement real-time activity feed
- [ ] Create system events timeline
- [ ] Add heatmap for error distribution
- [ ] Add gauge charts for key metrics

**7.2 User Experience Metrics**
- [ ] Track average page load time
- [ ] Monitor SignalR connection quality
- [ ] Track browser compatibility stats
- [ ] Monitor error rate by page
- [ ] Track average session duration
- [ ] Display user experience score

**7.3 Comparison & Benchmarks**
- [ ] Store baseline metrics
- [ ] Compare current vs. baseline
- [ ] Add threshold indicators
- [ ] Show recommended values
- [ ] Add industry benchmarks (if available)
- [ ] Highlight anomalies

**7.4 Developer Tools (Admin Only)**
- [ ] Add "View Recent Logs" panel
- [ ] Display recent exceptions with stack traces
- [ ] Show API request log
- [ ] Display slow database queries
- [ ] Add "Enable Debug Mode" toggle
- [ ] Add "Clear Logs" action

**7.5 Mobile Optimization**
- [ ] Optimize layout for mobile devices
- [ ] Implement swipe gestures for navigation
- [ ] Add mobile-specific interactions
- [ ] Test on various screen sizes

**7.6 Accessibility**
- [ ] Add ARIA labels to all interactive elements
- [ ] Ensure keyboard navigation works
- [ ] Test with screen readers
- [ ] Add high contrast mode support
- [ ] Ensure color blindness compatibility

**7.7 Performance Optimization**
- [ ] Implement lazy loading for charts
- [ ] Optimize data fetching (only load visible sections)
- [ ] Add caching for historical data
- [ ] Minimize re-renders
- [ ] Optimize bundle size

---

## Technical Implementation Details

### Real-time Updates Architecture
```csharp
// Auto-refresh timer in SystemDiagnostics.razor
private Timer? _refreshTimer;
private DateTime _lastUpdated;
private bool _isRefreshing;

protected override void OnInitialized()
{
    _refreshTimer = new Timer(30000); // 30 seconds
    _refreshTimer.Elapsed += async (sender, e) => await RefreshDiagnostics();
    _refreshTimer.Start();
}

private async Task RefreshDiagnostics()
{
    _isRefreshing = true;
    await InvokeAsync(StateHasChanged);
    
    _diagnostics = await DiagnosticsService.GetCompleteDiagnosticsAsync();
    _lastUpdated = DateTime.Now;
    _isRefreshing = false;
    
    await InvokeAsync(StateHasChanged);
}
```

### Health Status Enum
```csharp
public enum HealthStatus
{
    Unknown,
    Healthy,
    Warning,
    Error,
    Critical
}
```

### Diagnostic Alert Model
```csharp
public class DiagnosticAlert
{
    public int Id { get; set; }
    public string Category { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public AlertSeverity Severity { get; set; }
    public DateTime DetectedAt { get; set; }
    public DateTime? AcknowledgedAt { get; set; }
    public string? AcknowledgedBy { get; set; }
    public bool IsActive { get; set; }
    public string? RecommendedAction { get; set; }
}

public enum AlertSeverity
{
    Info,
    Warning,
    Error,
    Critical
}
```

### Performance Metrics Model
```csharp
public class PerformanceMetrics
{
    public long MemoryUsedBytes { get; set; }
    public long MemoryMaxBytes { get; set; }
    public double MemoryUsagePercent => (double)MemoryUsedBytes / MemoryMaxBytes * 100;
    
    public double CpuUsagePercent { get; set; }
    public TimeSpan Uptime { get; set; }
    public int ActiveConnections { get; set; }
    public int RequestsPerMinute { get; set; }
    public int ErrorCount24Hours { get; set; }
    
    public HealthStatus OverallHealth => CalculateHealth();
    
    private HealthStatus CalculateHealth()
    {
        if (ErrorCount24Hours > 100 || MemoryUsagePercent > 90 || CpuUsagePercent > 90)
            return HealthStatus.Critical;
        if (ErrorCount24Hours > 50 || MemoryUsagePercent > 75 || CpuUsagePercent > 75)
            return HealthStatus.Warning;
        return HealthStatus.Healthy;
    }
}
```

### Historical Data Storage
```csharp
// Store metrics every minute for trending
public class DiagnosticSnapshot
{
    public int Id { get; set; }
    public DateTime Timestamp { get; set; }
    public long MemoryUsed { get; set; }
    public double CpuUsage { get; set; }
    public int ActiveConnections { get; set; }
    public int ErrorCount { get; set; }
    public string? AdditionalData { get; set; } // JSON for flexibility
}
```

---

## UI/UX Design Specifications

### Color Palette
```css
:root {
    /* Status Colors */
    --status-healthy: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
    --status-warning: linear-gradient(135deg, #f093fb 0%, #f5576c 100%);
    --status-error: linear-gradient(135deg, #fa709a 0%, #fee140 100%);
    --status-unknown: linear-gradient(135deg, #a8edea 0%, #fed6e3 100%);
    
    /* Accent Colors */
    --accent-primary: #667eea;
    --accent-secondary: #764ba2;
    --accent-success: #10b981;
    --accent-warning: #f59e0b;
    --accent-danger: #ef4444;
    
    /* Backgrounds */
    --bg-card: rgba(255, 255, 255, 0.05);
    --bg-card-hover: rgba(255, 255, 255, 0.1);
    --bg-glass: rgba(255, 255, 255, 0.1);
    
    /* Shadows */
    --shadow-sm: 0 2px 4px rgba(0, 0, 0, 0.1);
    --shadow-md: 0 4px 6px rgba(0, 0, 0, 0.1);
    --shadow-lg: 0 10px 15px rgba(0, 0, 0, 0.1);
    --shadow-glow: 0 0 20px rgba(102, 126, 234, 0.3);
}
```

### Animations
```css
/* Pulse animation for status indicators */
@keyframes pulse {
    0%, 100% { opacity: 1; }
    50% { opacity: 0.5; }
}

/* Shimmer for skeleton loaders */
@keyframes shimmer {
    0% { background-position: -1000px 0; }
    100% { background-position: 1000px 0; }
}

/* Fade in animation */
@keyframes fadeIn {
    from { opacity: 0; transform: translateY(10px); }
    to { opacity: 1; transform: translateY(0); }
}

/* Count up animation for numbers */
@keyframes countUp {
    from { transform: scale(1.2); }
    to { transform: scale(1); }
}

.status-badge.healthy {
    animation: pulse 2s ease-in-out infinite;
}

.skeleton-loader {
    background: linear-gradient(90deg, #f0f0f0 25%, #e0e0e0 50%, #f0f0f0 75%);
    background-size: 1000px 100%;
    animation: shimmer 2s infinite;
}

.metric-card {
    animation: fadeIn 0.5s ease-out;
}
```

### Component Examples

**StatusBadge.razor**
```razor
<span class="status-badge @StatusClass" title="@Tooltip">
    <span class="status-dot"></span>
    @StatusText
</span>

@code {
    [Parameter] public HealthStatus Status { get; set; }
    [Parameter] public string? CustomText { get; set; }
    
    private string StatusClass => Status switch
    {
        HealthStatus.Healthy => "healthy",
        HealthStatus.Warning => "warning",
        HealthStatus.Error => "error",
        HealthStatus.Critical => "critical",
        _ => "unknown"
    };
    
    private string StatusText => CustomText ?? Status.ToString();
    
    private string Tooltip => Status switch
    {
        HealthStatus.Healthy => "System is operating normally",
        HealthStatus.Warning => "Minor issues detected",
        HealthStatus.Error => "Errors detected, attention needed",
        HealthStatus.Critical => "Critical issues, immediate action required",
        _ => "Status unknown"
    };
}
```

**MetricCard.razor**
```razor
<div class="metric-card @SizeClass" @onclick="OnClick">
    <div class="metric-icon">
        <i class="@IconClass"></i>
    </div>
    <div class="metric-content">
        <div class="metric-label">@Label</div>
        <div class="metric-value">
            @if (IsLoading)
            {
                <div class="skeleton-loader"></div>
            }
            else
            {
                <AnimatedCounter Value="@NumericValue" />
                <span class="metric-unit">@Unit</span>
            }
        </div>
        @if (!string.IsNullOrEmpty(SubText))
        {
            <div class="metric-subtext">@SubText</div>
        }
    </div>
    <StatusBadge Status="@Status" />
</div>

@code {
    [Parameter] public string Label { get; set; } = "";
    [Parameter] public string IconClass { get; set; } = "";
    [Parameter] public double NumericValue { get; set; }
    [Parameter] public string Unit { get; set; } = "";
    [Parameter] public string? SubText { get; set; }
    [Parameter] public HealthStatus Status { get; set; }
    [Parameter] public bool IsLoading { get; set; }
    [Parameter] public string Size { get; set; } = "medium"; // small, medium, large
    [Parameter] public EventCallback OnClick { get; set; }
    
    private string SizeClass => $"size-{Size}";
}
```

---

## Database Schema Updates

### DiagnosticSnapshots Table
```sql
CREATE TABLE DiagnosticSnapshots (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Timestamp DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    MemoryUsedBytes BIGINT NOT NULL,
    CpuUsagePercent DECIMAL(5,2) NOT NULL,
    ActiveConnections INT NOT NULL,
    ErrorCount INT NOT NULL,
    RequestsPerMinute INT NOT NULL,
    AdditionalData NVARCHAR(MAX) NULL, -- JSON
    INDEX IX_Timestamp (Timestamp DESC)
);
```

### DiagnosticAlerts Table
```sql
CREATE TABLE DiagnosticAlerts (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Category NVARCHAR(100) NOT NULL,
    Title NVARCHAR(200) NOT NULL,
    Description NVARCHAR(MAX) NOT NULL,
    Severity NVARCHAR(20) NOT NULL, -- Info, Warning, Error, Critical
    DetectedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    AcknowledgedAt DATETIME2 NULL,
    AcknowledgedBy NVARCHAR(256) NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    RecommendedAction NVARCHAR(MAX) NULL,
    INDEX IX_IsActive_Severity (IsActive, Severity)
);
```

### BackgroundJobHistory Table
```sql
CREATE TABLE BackgroundJobHistory (
    Id INT PRIMARY KEY IDENTITY(1,1),
    JobName NVARCHAR(200) NOT NULL,
    StartedAt DATETIME2 NOT NULL,
    CompletedAt DATETIME2 NULL,
    Status NVARCHAR(20) NOT NULL, -- Pending, Running, Completed, Failed
    ErrorMessage NVARCHAR(MAX) NULL,
    Duration INT NULL, -- milliseconds
    INDEX IX_JobName_StartedAt (JobName, StartedAt DESC)
);
```

---

## Testing Strategy

### Unit Tests
- Test each diagnostic service independently
- Mock dependencies (database, controllers, APIs)
- Verify health status calculations
- Test alert detection logic
- Verify data validation rules

### Integration Tests
- Test end-to-end diagnostic data collection
- Verify database connectivity checks
- Test controller communication
- Verify camera system diagnostics
- Test export functionality

### UI Tests
- Test component rendering
- Verify animations work smoothly
- Test responsive design on various screen sizes
- Verify accessibility features
- Test real-time updates

### Performance Tests
- Measure diagnostic data collection time
- Test with large historical datasets
- Verify auto-refresh doesn't cause memory leaks
- Test concurrent user access

---

## Deployment Checklist

- [ ] All phases completed and tested
- [ ] Database migrations applied
- [ ] Configuration settings documented
- [ ] User documentation created
- [ ] Admin training materials prepared
- [ ] Performance benchmarks established
- [ ] Monitoring alerts configured
- [ ] Backup and recovery tested
- [ ] Security audit completed
- [ ] Accessibility compliance verified

---

## Success Metrics

### User Experience
- Page load time < 2 seconds
- Auto-refresh without noticeable lag
- Smooth animations (60fps)
- Mobile responsive on all devices
- Accessibility score > 95%

### Functionality
- All diagnostic categories implemented
- Real-time updates working
- Alerts triggering correctly
- Export functionality working
- Actions executing successfully

### Business Value
- Reduced time to diagnose issues
- Proactive issue detection
- Improved system uptime
- Better visibility into system health
- Reduced support tickets

---

## Future Enhancements (Post-Launch)

- **AI-Powered Diagnostics**: Use machine learning to predict issues
- **Anomaly Detection**: Automatically detect unusual patterns
- **Predictive Maintenance**: Forecast when components may fail
- **Mobile App**: Dedicated mobile app for diagnostics
- **Voice Alerts**: Audio notifications for critical issues
- **Integration with Monitoring Tools**: Connect to Application Insights, Datadog, etc.
- **Custom Dashboards**: Allow users to create custom diagnostic views
- **API Endpoints**: Expose diagnostics via REST API
- **Webhooks**: Send diagnostic data to external systems
- **Multi-Tenant Support**: Diagnostics for multiple GFC installations

---

## Notes

- **Phased Approach**: Implement in phases to deliver value incrementally
- **User Feedback**: Gather feedback after each phase
- **Iterative Improvement**: Continuously refine based on usage patterns
- **Documentation**: Keep documentation updated as features are added
- **Performance**: Monitor performance impact of diagnostics collection
- **Security**: Ensure diagnostic data doesn't expose sensitive information

---

**Status**: Ready for Implementation
**Priority**: High
**Estimated Timeline**: 6-8 weeks (all phases)
**Dependencies**: None (can start immediately)
