// PWA Installer - Handles install prompt detection and triggering
// Supports iOS Safari, Android Chrome, and Desktop Chrome/Edge

window.pwaInstaller = (function () {
    let deferredPrompt = null;
    let isStandalone = false;

    // Detect if already running as installed PWA
    function checkIfStandalone() {
        // Check various standalone indicators
        const isStandalonePWA =
            window.matchMedia('(display-mode: standalone)').matches ||
            window.navigator.standalone === true ||
            document.referrer.includes('android-app://');

        return isStandalonePWA;
    }

    // Detect platform
    function detectPlatform() {
        const ua = navigator.userAgent.toLowerCase();
        const isIOS = /iphone|ipad|ipod/.test(ua);
        const isAndroid = /android/.test(ua);
        const isSafari = /safari/.test(ua) && !/chrome/.test(ua);
        const isChrome = /chrome/.test(ua);
        const isEdge = /edg/.test(ua);

        return {
            isIOS,
            isAndroid,
            isSafari,
            isChrome,
            isEdge,
            isDesktop: !isIOS && !isAndroid
        };
    }

    // Get platform-specific manual instructions
    function getManualInstructions(platform) {
        if (platform.isIOS && platform.isSafari) {
            return `
                1. Tap the <strong>Share</strong> button <i class="bi bi-box-arrow-up"></i><br>
                2. Scroll down and tap <strong>"Add to Home Screen"</strong><br>
                3. Tap <strong>"Add"</strong> to confirm
            `;
        }

        if (platform.isAndroid) {
            return `
                1. Tap the menu <strong>⋮</strong> in the top-right corner<br>
                2. Tap <strong>"Install app"</strong> or <strong>"Add to Home screen"</strong><br>
                3. Tap <strong>"Install"</strong> to confirm
            `;
        }

        if (platform.isDesktop && (platform.isChrome || platform.isEdge)) {
            return `
                1. Look for the install icon <strong>⊕</strong> in the address bar<br>
                2. Click it and select <strong>"Install"</strong><br>
                3. The app will open in a new window
            `;
        }

        return null;
    }

    // Initialize - capture the beforeinstallprompt event
    function init() {
        isStandalone = checkIfStandalone();

        // Check for early-captured prompt
        if (window._pwaBeforeInstallPrompt) {
            console.log('[PWA Installer] Recovering early-captured prompt');
            deferredPrompt = window._pwaBeforeInstallPrompt;
        }

        window.addEventListener('beforeinstallprompt', (e) => {
            console.log('[PWA Installer] Install prompt available');
            e.preventDefault();
            deferredPrompt = e;
        });

        window.addEventListener('appinstalled', () => {
            console.log('[PWA Installer] App installed successfully');
            deferredPrompt = null;
            isStandalone = true;
        });
    }

    // Check if installation is possible
    function checkInstallability() {
        // If already installed, don't show prompt
        if (isStandalone) {
            console.log('[PWA Installer] Already running as installed app');
            return {
                canInstall: false,
                instructions: null
            };
        }

        const platform = detectPlatform();

        // If we have a deferred prompt (Chrome/Edge), we can trigger it
        if (deferredPrompt) {
            return {
                canInstall: true,
                instructions: null
            };
        }

        // Otherwise, provide manual instructions based on platform
        const instructions = getManualInstructions(platform);

        return {
            canInstall: false,
            instructions: instructions
        };
    }

    // Trigger the install prompt
    async function install() {
        // Run diagnostics first
        const diagnostics = await runDiagnostics();

        if (!diagnostics.allPassed) {
            console.error('[PWA Installer] Installation requirements not met:', diagnostics);

            // Show user-friendly error message
            const issues = [];
            if (!diagnostics.isHttps) issues.push('Site must be served over HTTPS');
            if (!diagnostics.hasServiceWorker) issues.push('Service Worker not registered');
            if (!diagnostics.hasManifest) issues.push('Web App Manifest not found');
            if (!diagnostics.hasIcons) issues.push('Required icons not found');

            alert('Cannot install app:\n\n' + issues.join('\n') + '\n\nPlease contact support.');
            return false;
        }

        if (!deferredPrompt) {
            console.log('[PWA Installer] No install prompt available - showing manual instructions');

            const platform = detectPlatform();
            const instructions = getManualInstructions(platform);

            if (instructions) {
                // Create a modal with instructions
                const modal = document.createElement('div');
                modal.style.cssText = 'position:fixed;top:0;left:0;right:0;bottom:0;background:rgba(0,0,0,0.9);z-index:99999;display:flex;align-items:center;justify-content:center;padding:20px;';
                modal.innerHTML = `
                    <div style="background:white;padding:30px;border-radius:16px;max-width:400px;text-align:center;">
                        <h3 style="margin-top:0;color:#333;">Install GFC App</h3>
                        <div style="text-align:left;color:#666;line-height:1.8;margin:20px 0;">
                            ${instructions}
                        </div>
                        <button onclick="this.closest('div').parentElement.remove()" 
                                style="background:#0d6efd;color:white;border:none;padding:12px 24px;border-radius:8px;font-weight:600;cursor:pointer;">
                            Got it
                        </button>
                    </div>
                `;
                document.body.appendChild(modal);
            } else {
                alert('Installation not available on this browser. Please use Chrome, Edge, or Safari.');
            }

            return false;
        }

        try {
            console.log('[PWA Installer] Showing install prompt...');

            // Show the install prompt
            deferredPrompt.prompt();

            // Wait for the user to respond
            const choiceResult = await deferredPrompt.userChoice;

            if (choiceResult.outcome === 'accepted') {
                console.log('[PWA Installer] User accepted the install prompt');
                deferredPrompt = null;
                return true;
            } else {
                console.log('[PWA Installer] User dismissed the install prompt');
                return false;
            }
        } catch (error) {
            console.error('[PWA Installer] Error triggering install:', error);
            alert('Installation failed: ' + error.message);
            return false;
        }
    }

    // Run diagnostic checks
    async function runDiagnostics() {
        const results = {
            isHttps: false,
            hasServiceWorker: false,
            hasManifest: false,
            hasIcons: false,
            allPassed: false
        };

        // Check HTTPS (localhost is exempt)
        results.isHttps = location.protocol === 'https:' || location.hostname === 'localhost';

        // Check Service Worker
        if ('serviceWorker' in navigator) {
            try {
                const registration = await navigator.serviceWorker.getRegistration();
                results.hasServiceWorker = !!registration;
            } catch (e) {
                console.error('[PWA Diagnostics] Service Worker check failed:', e);
            }
        }

        // Check Manifest
        try {
            const manifestLink = document.querySelector('link[rel="manifest"]');
            if (manifestLink) {
                const response = await fetch(manifestLink.href);
                if (response.ok) {
                    const manifest = await response.json();
                    results.hasManifest = true;

                    // Check if icons exist
                    if (manifest.icons && manifest.icons.length > 0) {
                        // Try to fetch at least one icon
                        const iconResponse = await fetch(manifest.icons[0].src);
                        results.hasIcons = iconResponse.ok;
                    }
                }
            }
        } catch (e) {
            console.error('[PWA Diagnostics] Manifest check failed:', e);
        }

        results.allPassed = results.isHttps && results.hasServiceWorker && results.hasManifest && results.hasIcons;

        console.log('[PWA Diagnostics]', results);
        return results;
    }

    // Public API
    return {
        init,
        checkInstallability,
        install
    };
})();

// Initialize on load
if (document.readyState === 'loading') {
    document.addEventListener('DOMContentLoaded', () => pwaInstaller.init());
} else {
    pwaInstaller.init();
}
