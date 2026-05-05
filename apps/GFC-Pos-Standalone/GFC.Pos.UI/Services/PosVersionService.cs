using GFC.Core.Interfaces;

namespace GFC.Pos.UI.Services;

public class PosVersionService : IVersionService
{
    public string GetDeveloper() => "GFC";
    public string GetYear() => "2026";
    public string GetRevision() => "2.31.0"; 
    
    // REVISION HISTORY:
    // 2.31.0: Finalized production API targeting and CORS configuration for pos.lovanow.com.
    // 2.27.0: Centered Janitor Hours layout and improved header text visibility.
    // 2.26.0: Fixed payroll mapping issues and stabilized mobile reporting hub.
    // 2.25.0: Removed back navigation button from header for a cleaner UI.
    // 2.24.0: Improved USB permission handling and synchronized multi-project printer configuration logic.
    // 2.23.0: Added support for Ethernet (TCP/IP) printers and redesigned printer settings architecture.
    // 2.22.0: Modernized "No Items Found" UI with refined aesthetics and proper loading state logic.
    // 2.21.0: Simplified Correction Checkout UI (Removed numpad, focus on GIVE BACK/COLLECT)
    // 2.20.0: Implemented 'Last Transaction' bar and 'Correction Mode' with soft-void tracking
    // 2.19.0: Renamed 'REDEEM' button to 'REDEEM TOKEN'
    // 2.15.0: Differentiated (TOKEN SALE) vs (REDEEMED) logic and color-coding
    // 2.11.0: Toned down Checkout UI with professional light-themed display
    // 2.10.0: Extreme Visibility Scaling for Checkout Display
    // 2.9.0: Added USB Printer Auto-Scan and Redesigned Checkout UI Display
    // 2.8.0: Added Direct USB Thermal Printing and Settings UI
    // 2.7.0: Initial Standalone POS Release

    public string GetFullVersion() => $"GFC POS Standalone v{GetRevision()}";
}
