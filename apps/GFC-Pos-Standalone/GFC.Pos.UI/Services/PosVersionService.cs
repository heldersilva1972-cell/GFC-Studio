using GFC.Core.Interfaces;

namespace GFC.Pos.UI.Services;

public class PosVersionService : IVersionService
{
    public string GetDeveloper() => "GFC";
    public string GetYear() => "2026";
    public string GetRevision() => "2.38.58"; 
    
    // REVISION HISTORY:
    // 2.38.58: Implemented Sticky Immersive Mode on Android terminals to automatically hide the navigation and status bars for a cleaner POS experience.
    // 2.38.57: Refactored Banquet 'Add Funds' API to use a robust DTO and body-based POST for improved reliability on Android terminals.
    // 2.38.56: Added Banquet Tab management directly to POS terminal with automatic Z-Report deposit logging.
    // 2.38.55: Improved Event Tab insufficient funds warning clarity and dynamic button text.
    // 2.38.54: Added Event Tabs checkout support and active event selector.
    // 2.38.43: Synchronized version.txt and version.json to fix the 'Update Available' banner visibility issue.
    // 2.38.42: Compacted catalog items (product-card) and category filters (filter-pill) to maximize visibility on 15-inch displays.
    // 2.38.41: Scaled down Checkout window dimensions and content (font-sizes/padding) for better fit on compact displays.
    // 2.38.40: Reduced Total Due font-size and compacted Active Order list (44px items) to fit 9+ items. Restored Checkout/Redeem buttons to premium default sizes.
    // 2.38.39: Hardened update pill with div-based rendering and CSS cache-busting to fix square-box issue and persistent visibility.
    // 2.38.38: Applied extreme vertical space reclamation across header, catalog padding, and footer buttons. Items now 22px.
    // 2.38.37: Further compacted Active Order list items and header to maximize item visibility.
    // 2.38.36: Hardened Update Available pill style with aggressive rounding and padding to prevent square rendering.
    // 2.38.35: Implemented premium animated update pill and ultra-compact Total Due area. Hardened cache-clearing reload logic.
    // 2.38.34: Further compacted Active Order items and Total Due box for better visibility on 15-inch Android terminals.
    // 2.38.33: Restored deployment paths to C:\inetpub\wwwroot and fixed script syntax errors. Synchronized versioning across all tracks.
    // 2.38.32: Resolved terminal infinite update loop by automating index.html cache-buster synchronization and server-side stale file cleanup.
    // 2.38.7: Migrated Terminal Name storage to native device preferences to survive browser cache clearing.
    // 2.38.6: Compacted Active Order display and Total Due area for better screen utilization. Fixed header double terminal name.
    // 2.38.5: Implemented background version polling to ensure idle terminals receive updates automatically.
    // 2.38.4: Corrected Lottery financial math to properly account for Envelope Drops and Bag Refills in variance calculations.
    // 2.38.0: Refined Bingo financial logic (Standard game 0% club share) and synchronized suite-wide revisions.
    // 2.37.0: Standardized Bingo Progressive entry and implemented forced program synchronization for mobile floor staff.
    // 2.36.0: Automated background menu refresh upon terminal lock (idle, security, or manual) to ensure data parity.
    // 2.35.0: Added 'CANCEL CORRECTION' button to allow exiting correction mode without saving changes.
    // 2.34.0: Implemented smart API redirection for localhost and added environment status labels to header.
    // 2.33.0: Fixed Argument_InvalidHandle by hardening IIS compression rules and registering Service Worker.
    // 2.32.0: Added .dat to Service Worker manifest and synchronized versioning to resolve 404 startup hangs.
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
    public string GetMobileVersion() => GetFullVersion();
    public string GetPosVersion() => GetFullVersion();
}




























