// GFC Analytics Tracker
// Automatically tracks page views, time on page, and user activity

window.GFC_Analytics = (function () {
    let currentPageUrl = null;
    let pageStartTime = null;
    let heartbeatInterval = null;
    let isTracking = false;

    // Configuration
    const HEARTBEAT_INTERVAL_MS = 30000; // 30 seconds
    const API_ENDPOINT = '/api/analytics';

    // Initialize tracking
    function init() {
        console.log('[Analytics] Initializing tracker...');

        // Track initial page
        trackPageView(window.location.pathname);

        // Listen for Blazor navigation events
        if (window.Blazor) {
            Blazor.addEventListener('enhancedload', () => {
                trackPageView(window.location.pathname);
            });
        }

        // Fallback: Listen for URL changes
        let lastUrl = window.location.pathname;
        setInterval(() => {
            const currentUrl = window.location.pathname;
            if (currentUrl !== lastUrl) {
                lastUrl = currentUrl;
                trackPageView(currentUrl);
            }
        }, 1000);

        // Track when user leaves the page
        window.addEventListener('beforeunload', () => {
            stopTracking();
        });

        // Track visibility changes (tab switching)
        document.addEventListener('visibilitychange', () => {
            if (document.hidden) {
                stopHeartbeat();
            } else {
                startHeartbeat();
            }
        });

        console.log('[Analytics] Tracker initialized');
    }

    // Track a page view
    function trackPageView(pageUrl) {
        // Strip leading slash to match Blazor's relative URL format
        if (pageUrl.startsWith('/')) {
            pageUrl = pageUrl.substring(1);
        }
        if (!pageUrl || pageUrl === '') pageUrl = 'Dashboard';

        // Stop tracking previous page
        if (isTracking) {
            stopTracking();
        }

        currentPageUrl = pageUrl;
        pageStartTime = Date.now();
        isTracking = true;

        console.log('[Analytics] Page view:', pageUrl);

        // Send initial page view event
        sendEvent({
            action: 'PageView',
            pageUrl: pageUrl,
            details: `Navigated to ${getPageName(pageUrl)}`,
            durationSeconds: 0
        });

        // Start heartbeat to track time on page
        startHeartbeat();
    }

    // Start sending heartbeats
    function startHeartbeat() {
        if (heartbeatInterval) {
            clearInterval(heartbeatInterval);
        }

        heartbeatInterval = setInterval(() => {
            if (isTracking && currentPageUrl && pageStartTime) {
                const durationSeconds = Math.floor((Date.now() - pageStartTime) / 1000);

                // Send heartbeat to update duration
                sendHeartbeat(currentPageUrl, 30); // 30 seconds increment

                console.log('[Analytics] Heartbeat:', currentPageUrl, durationSeconds + 's');
            }
        }, HEARTBEAT_INTERVAL_MS);
    }

    // Stop heartbeat
    function stopHeartbeat() {
        if (heartbeatInterval) {
            clearInterval(heartbeatInterval);
            heartbeatInterval = null;
        }
    }

    // Stop tracking current page
    function stopTracking() {
        if (!isTracking) return;

        stopHeartbeat();

        // Send final duration update
        if (currentPageUrl && pageStartTime) {
            const durationSeconds = Math.floor((Date.now() - pageStartTime) / 1000);
            sendHeartbeat(currentPageUrl, durationSeconds % 30); // Send remaining seconds
        }

        isTracking = false;
        currentPageUrl = null;
        pageStartTime = null;
    }

    // Send analytics event
    async function sendEvent(data) {
        try {
            await fetch(`${API_ENDPOINT}/track`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(data)
            });
        } catch (error) {
            console.error('[Analytics] Failed to send event:', error);
        }
    }

    // Send heartbeat to update duration
    async function sendHeartbeat(pageUrl, additionalSeconds) {
        // Double check leading slash normalization
        if (pageUrl.startsWith('/')) {
            pageUrl = pageUrl.substring(1);
        }
        if (!pageUrl || pageUrl === '') pageUrl = 'Dashboard';

        try {
            await fetch(`${API_ENDPOINT}/heartbeat`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({
                    pageUrl: pageUrl,
                    additionalSeconds: additionalSeconds
                })
            });
        } catch (error) {
            console.error('[Analytics] Failed to send heartbeat:', error);
        }
    }

    // Track custom action
    function trackAction(action, details, pageUrl = null) {
        let targetUrl = pageUrl || currentPageUrl || window.location.pathname;
        if (targetUrl.startsWith('/')) {
            targetUrl = targetUrl.substring(1);
        }
        if (!targetUrl || targetUrl === '') targetUrl = 'Dashboard';

        sendEvent({
            action: action,
            pageUrl: targetUrl,
            details: details,
            durationSeconds: 0
        });

        console.log('[Analytics] Action:', action, details);
    }

    // Get friendly page name from URL
    function getPageName(url) {
        // Normalize URL first
        if (url.startsWith('/')) {
            url = url.substring(1);
        }
        if (!url || url === '') return 'Dashboard';
        if (url === 'login') return 'Login';
        if (url === 'mobile') return 'Mobile Hub';
        if (url.startsWith('admin/')) {
            const page = url.replace('admin/', '').replace(/-/g, ' ');
            return page.split('/').map(s => s.charAt(0).toUpperCase() + s.slice(1)).join(' - ');
        }

        // Default: capitalize and format
        return url.split('/').map(segment => {
            return segment.split('-').map(word =>
                word.charAt(0).toUpperCase() + word.slice(1)
            ).join(' ');
        }).join(' - ');
    }

    // Public API
    return {
        init,
        trackAction,
        trackPageView
    };
})();

// Auto-initialize when DOM is ready
if (document.readyState === 'loading') {
    document.addEventListener('DOMContentLoaded', () => GFC_Analytics.init());
} else {
    GFC_Analytics.init();
}
