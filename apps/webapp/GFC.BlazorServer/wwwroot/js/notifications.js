/**
 * GFC Push Notifications Interop
 * Handles Browser Push API registration and Service Worker communication
 */

window.GFC_Notifications = {
    /**
     * Requests browser permission to show notifications
     * @returns {Promise<string>} 'granted', 'denied', or 'default'
     */
    requestPermission: async function () {
        console.log('[Notifications] Requesting permission...');

        if (!('Notification' in window)) {
            console.warn('[Notifications] API not supported in this browser.');
            return 'not_supported';
        }

        if (!window.isSecureContext) {
            console.warn('[Notifications] Insecure context (HTTP). HTTPS required.');
            return 'insecure_context';
        }

        console.log('[Notifications] Current permission state:', Notification.permission);

        if (Notification.permission === 'granted') {
            return 'granted';
        }

        if (Notification.permission === 'denied') {
            console.warn('[Notifications] Permission is blocked by browser/OS.');
            return 'denied';
        }

        try {
            console.log('[Notifications] Triggering browser prompt...');
            const permission = await Notification.requestPermission();
            console.log('[Notifications] User decision:', permission);
            return permission;
        } catch (error) {
            console.error('[Notifications] Permission request error:', error);
            return 'denied';
        }
    },

    /**
     * Binds a native click listener to a button to guarantee User Activation (Gesture).
     * Back to DIRECT binding for performance and reliability.
     */
    bindNativePrompt: function (buttonId, vapidKey, dotNetRef) {
        const btn = document.getElementById(buttonId);
        if (!btn) return;

        // Store globals for callback access
        window._gfcVapidKey = vapidKey;
        window._gfcDotNetRef = dotNetRef;

        // 1. Kill any existing listener to prevent stacking
        btn.onclick = null;

        btn.onclick = async function (e) {
            // 1. Hard Execution Gate
            const now = Date.now();
            if (window.__GFC_GATE__ && (now - window.__GFC_GATE__ < 5000)) return;
            window.__GFC_GATE__ = now;

            e.preventDefault();
            e.stopImmediatePropagation();

            window.__gfcNotifDebug?.(`CLICK: Processing subscription...`);

            const originalContent = btn.innerHTML;
            btn.disabled = true;
            btn.innerHTML = '<span class="spinner-border spinner-border-sm me-2"></span>Wait...';

            let bridgeHandoff = false;

            try {
                // Normal subscription path
                const subJson = await window.GFC_Notifications.subscribe(vapidKey);
                if (dotNetRef) await dotNetRef.invokeMethodAsync('OnSubscriptionSuccess', subJson);
            } catch (err) {
                console.error('[GFC] Flow Error:', err);
                let msg = err.message || 'Unknown error';
                if (msg.includes('Push Server Error')) {
                    msg = "Push Server Error: The browser cannot connect to the push notification service. Check your internet/VPN or try disabling AdBlock.";
                }
                if (dotNetRef) await dotNetRef.invokeMethodAsync('OnSubscriptionError', msg);
            } finally {
                // [FIXED] Explicitly track handoff - do NOT read Notification.permission here
                const isWaitingOnBridge = bridgeHandoff === true;
                if (!isWaitingOnBridge) {
                    btn.disabled = false;
                    btn.innerHTML = originalContent;
                    window.__GFC_GATE__ = 0;
                }
            }
        };
    },

    /**
     * Subscribes the user to push notifications via the Service Worker
     */
    subscribe: async function (vapidPublicKey) {
        // [OPTION A] Pure PWA Mode: Always allow browser prompts
        const currentPerm = Notification.permission;
        if (window.Notification && currentPerm !== 'granted') {
            console.log('[GFC] Current permission:', currentPerm);

            // If denied, don't even try to prompt, it will fail silently
            if (currentPerm === 'denied') {
                throw new Error('Notification permission is DENIED at the browser level. Please reset permissions in your address bar.');
            }

            console.log('[GFC] Requesting browser notification permission...');
            window.__gfcNotifDebug?.('CALL: Notification.requestPermission() (BROWSER)');

            // Race the permission prompt with a timeout to avoid hanging the button on "Wait..."
            const result = await Promise.race([
                Notification.requestPermission(),
                new Promise((_, reject) => setTimeout(() => reject(new Error('Notification prompt timed out or was ignored.')), 10000))
            ]);

            console.log('[GFC] Permission result:', result);
            if (result !== 'granted') {
                throw new Error(`Permission ${result}. Subscriptions require "granted" status.`);
            }
        }

        if (!('serviceWorker' in navigator)) {
            throw new Error('Service Worker not supported');
        }

        try {
            // Wait for SW with a timeout - prevents "doing nothing" if SW fails
            console.log('[Push] Waiting for service worker ready...');
            const registration = await Promise.race([
                navigator.serviceWorker.ready,
                new Promise((_, reject) => setTimeout(() => reject(new Error('Timed out waiting for Service Worker. Please refresh.')), 6000))
            ]);

            // Force fresh subscription
            const existing = await registration.pushManager.getSubscription();
            if (existing) {
                console.log('[Push] Resetting stale subscription...');
                await existing.unsubscribe();
            }

            const convertedVapidKey = this.urlBase64ToUint8Array(vapidPublicKey);

            const subscription = await registration.pushManager.subscribe({
                userVisibleOnly: true,
                applicationServerKey: convertedVapidKey
            });

            console.log('[Push] Fresh subscription created.');
            return JSON.stringify(subscription);
        } catch (error) {
            console.error('[Push] Subscribe failed:', error);
            window._gfcLastNotifError = error.message || error.toString();
            throw error;
        }
    },

    /**
     * Unsubscribes the user from push notifications
     */
    unsubscribe: async function () {
        if (!('serviceWorker' in navigator)) return false;

        try {
            const registration = await navigator.serviceWorker.ready;
            const subscription = await registration.pushManager.getSubscription();

            if (subscription) {
                const successful = await subscription.unsubscribe();
                console.log('[Push] Unsubscribed:', successful);
                return successful;
            }
            return true;
        } catch (error) {
            console.error('[Push] Unsubscribe failed:', error);
            return false;
        }
    },

    /**
     * Checks if the user is currently subscribed
     */
    getSubscription: async function () {
        if (!('serviceWorker' in navigator)) return false;

        try {
            const registration = await navigator.serviceWorker.ready;
            const subscription = await registration.pushManager.getSubscription();
            return !!subscription;
        } catch (e) {
            return false;
        }
    },

    /**
     * Helper: Convert VAPID key to Uint8Array
     */
    urlBase64ToUint8Array: function (base64String) {
        if (!base64String) return new Uint8Array(0);
        base64String = base64String.trim();
        const padding = '='.repeat((4 - base64String.length % 4) % 4);
        const base64 = (base64String + padding)
            .replace(/\-/g, '+')
            .replace(/_/g, '/');

        const rawData = window.atob(base64);
        const outputArray = new Uint8Array(rawData.length);

        for (let i = 0; i < rawData.length; ++i) {
            outputArray[i] = rawData.charCodeAt(i);
        }
        return outputArray;
    }
};

// End of GFC_Notifications
