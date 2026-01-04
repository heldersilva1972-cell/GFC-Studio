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
        if (!deferredPrompt) {
            console.log('[PWA Installer] No install prompt available');
            return false;
        }

        try {
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
            return false;
        }
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
