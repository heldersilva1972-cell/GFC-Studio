// [CRITICAL] DO NOT EDIT GetRevision() OR GetVersion() MANUALLY. 
// Use the sync-version.ps1 script in the root directory.
// Manual edits are only allowed in the REVISION HISTORY section below.

using GFC.Core.Interfaces;

namespace GFC.Pos.UI.Services;

public class PosVersionService : IVersionService
{
    public string GetDeveloper() => "GFC";
    public string GetYear() => "2026";
    public string GetRevision() => "2.43.134"; 
    public string GetBuildDate() => "2026-05-16";
    public string GetChanges() => "Checkout UI & Sync Hardening (v2.43.4)";
    public string GetVersion() => "2.43.4";
    
    // REVISION HISTORY:
    // 2.43.4: Checkout UI & Sync Hardening (Large fonts + Vault bridge fix).
    // 2.43.2: Secure Architecture Hardening (Relative SignalR, No-SSL-Bypass, Offline-First Sync).
    // 2.43.1: Hardened Operator Sync (Fixed EF join issues for MemberIds).
    // 2.43.0: Added Emergency Login (Secret 5-tap on shield icon reveals PIN entry fallback).
    // 2.42.1: Finalized Operator Sync (Broadened server-side filter to include all staff with cards).
    // 2.42.0: Hardened Card Auth (Numeric comparison for leading zeros + operator list in diagnostics).
    // 2.41.9: Improved Card Auth Feedback (Show scanned card ID in warning).
    // 2.41.8: Fixed Toast Visibility (Elevated z-index to 30,000 to show above blurred lockscreen).
    // 2.41.7: Added Lockscreen Diagnostics (Tap 'GFC STANDALONE POS' on lockscreen 5 times).
    // 2.41.6: Hardened Card Scanner (Increased hardware timeout to 250ms for tablet compatibility).
    // 2.41.5: Fixed Checkout Button Layout (Stacked full-width buttons for clarity).
    // 2.41.4: Redesigned Checkout Keypad (Fixed layout and improved aesthetics).
    // 2.41.3: Extreme Checkout UI Compaction (Reclaimed vertical space).
    // 2.41.2: Compacted Checkout Keypad (Shorter Modal).
    // 2.41.1: Fixed Card-Tap Login JS initialization race condition.
    // 2.41.0: Implemented Instant Card-Tap Login via global HID listener.
    // 2.40.5: Fixed Checkout Keypad Proportions.
    // 2.40.4: Improved Token Upgrade Filtering.
    // 2.40.3: Fixed Script Leak in index.html.
    // 2.40.2: Fixed RowVersion Serialization Crash.
    // 2.40.1: Hydrate-First & Sync Guard.
    // 2.40.0: PWA Cache-First & LocalForage Vault.
    // 2.39.20: Indestructible LocalStorage Persistence.
    // 2.39.19: Dual-Layer Offline Persistence.
    // 2.39.18: Fixed Offline Vault Persistence.
    // 2.39.17: Truly Offline-First Boot Logic.
    // 2.39.16: Tightened Vertical Checkout UI.
    // 2.39.15: Compact Checkout UI.
    // 2.39.14: Compact 3-column token grid.
    // 2.39.13: Optimized token modal layout.
    // 2.39.12: Restored Token and Event data streams.
    // 2.39.11: Fixed PosToken nullability stability for WebApp & Terminal.
    // 2.39.10: Restored production UI (v2.39.10).
    // 2.38.99: Forced Service Worker activation (v2.38.99).
    // 2.38.98: Fixed category case-sensitivity filter bug (v2.38.98).
    // 2.38.96: Ensured Rescue Menu is always visible on empty catalog (v2.38.96).
    // 2.38.95: Fixed POS sync ghosting and added Rescue Menu (v2.38.95).
    // 2.38.94: Fixed CS0117 compilation error by adding 'DisplayOrder' property to the internal ProductItem model in PosTerminal.razor.
    // 2.38.93: Restored POS item visibility: defaulted new items to 'ShowInPos=true', added visibility toggles to Liquor Hub, and hardened terminal vault loading with increased timeouts and diagnostics.
    // 2.38.92: Updated versioning system to track minor increments across all deployment manifests.
    // 2.38.91: Fixed WebApp Transaction Audit 'Yesterday' filter error by synchronizing PosZReport database schema and optimizing EF Core date queries.
    // 2.38.90: Redesigned token redemption modal to present 'Standard' and 'Upgrade' as separate, descriptive cards for each token type.
    // 2.38.89: Decoupled 'Redeem' and 'Upgrade' into two distinct top-level buttons in the POS footer for improved workflow efficiency.
    // 2.38.88: Added dynamic audit descriptions to token credits (e.g., '> BEER TOKEN CREDIT FOR TITO'S') for transparent Z-Report tracking.
    // 2.38.87: Categorized all token redemptions under a dedicated 'TOKENS' section on the Z-Report and refined line item wording.
    // 2.38.86: Improved Z-Report financial transparency with clearer labels (SALES TOTAL vs NET CASH) and itemized credit breakdown.
    // 2.38.85: Hardened token logic to prevent double-crediting the same item. Items are now tagged as 'IsTokenApplied' upon redemption/upgrade.
    // 2.38.84: Implemented dual-mode redemption buttons (REDEEM ONE vs UPGRADE) in the token modal, restoring one-click speed for exact matches.
    // 2.38.83: Integrated Credit Upgrade item selection directly into the 'Redeem Tokens' modal for a faster, single-overlay workflow.
    // 2.38.82: Improved Credit Upgrade UI wording to 'UPGRADE [TOKEN] TO [ITEM]' for better bartender clarity.
    // 2.38.81: Added native support for dual-currency token reconciliation and improved offline synchronization buffers for RK3399 terminals.
    // 2.38.80: Prioritized Credit Upgrade selection overlay even when exact matches exist, providing manual control for Smart Tokens.
    // 2.38.79: Fixed Z-Report financial transparency (Token Credits display) and hardened token redemption logic for multi-item carts.
    // 2.38.78: Implemented Smart Token redemption system with category-restricted credit upgrades and detailed financial audit tracking.
    // 2.38.77: Optimized Banquet Manager to hide the 'Starting Cash Deposit' field for Running Tab events, enabling one-click event starts for non-prepaid tabs.
    // 2.38.76: Fixed banquet persistence (Vault-Sync on close), optimized Storage Worker DB initialization for offline startup, and eliminated global button latency via connectivity heartbeat caching.
    // 2.38.74: Integrated detailed banquet breakdown into Z-Reports, including individual deposit tracking and itemized tab expenditures.
    // 2.38.71: Optimized the 'Add Funds' modal with a compact 400px layout and streamlined padding for industrial touch terminals.
    // 2.38.70: Fixed CS0117 error by correctly referencing 'RunningTab' in the event pill logic.
    // 2.38.69: Fixed CS0019 compilation error by correcting enum type comparison in the header event pill.
    // 2.38.68: Standardized all UI labels to use 'Event' terminology instead of 'Banquet' for better consistency.
    // 2.38.67: Streamlined banquet logic: auto-selection of single active event in header and enforced single-banquet constraint in manager.
    // 2.38.66: Verified structural tag balance and resolved redundant closing divs in the POS terminal.
    // 2.38.65: Fixed structural Razor syntax errors (unclosed div and duplicate blocks) and verified build stability.
    // 2.38.64: Redesigned Banquet Manager with a non-scrolling two-column layout for optimized 15.6-inch terminal viewing.
    // 2.38.63: Implemented custom numeric keypad for Banquet Manager with auto-clear logic and forced spinner removal.
    // 2.38.62: Updated OPTIONS button style from black to premium navy blue to harmonize with the terminal header design.
    // 2.38.61: Finalized Industrial Immersive Mode for RK3399 hardware with Edge-to-Edge layout and transparent navigation fallbacks.
    // 2.38.60: Applied delayed enforcement (500ms) for Android Immersive Mode to prevent OS layout overrides on startup.
    // 2.38.59: Hardened Android Immersive Mode with support for modern WindowInsetsController API to ensure the navigation bar is hidden across all OS versions.
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





























































































































































































































