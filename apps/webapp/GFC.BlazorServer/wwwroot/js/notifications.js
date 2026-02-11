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
        const isNativeAPK = !!window.__GFC_NATIVE_APP__;

        if (isNativeAPK) {
            console.log('[Notifications] APK mode: browser permission prompt disabled.');
            return 'default';
        }

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

            window.__gfcNotifDebug?.(
                `CLICK: nativeFlag=${!!window.__GFC_NATIVE_APP__} androidBridge=${!!window.Android}`
            );

            const originalContent = btn.innerHTML;
            btn.disabled = true;
            btn.innerHTML = '<span class="spinner-border spinner-border-sm me-2"></span>Wait...';

            let bridgeHandoff = false;

            try {
                const isNativeAPK = !!window.__GFC_NATIVE_APP__;
                if (isNativeAPK && window.Android) {
                    let status = window.Android.getNotificationStatus();
                    if (typeof status === 'string') status = JSON.parse(status);

                    if (status && status.sdkInt >= 33 && !status.runtimeGranted) {
                        if (status.canPrompt === false) {
                            window.Android.openNotificationSettings();
                            window.__GFC_GATE__ = 0;
                            return;
                        }

                        console.log('[GFC] Handoff to Bridge...');
                        bridgeHandoff = true;
                        window.__gfcNotifDebug?.('CALL: Android.requestNotificationPermission()');
                        window.Android.requestNotificationPermission();

                        // Safety timeout to re-enable button if prompt is active
                        setTimeout(() => {
                            if (btn) { btn.disabled = false; btn.innerHTML = originalContent; }
                            window.__GFC_GATE__ = 0;
                        }, 5000);
                        return;
                    }
                }

                // Normal subscription path
                const subJson = await window.GFC_Notifications.subscribe(vapidKey);
                if (dotNetRef) await dotNetRef.invokeMethodAsync('OnSubscriptionSuccess', subJson);
            } catch (err) {
                console.error('[GFC] Flow Error:', err);
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
        // [FIX] Use multiple guards to ensure browser prompt NEVER fires in APK mode
        const isNativeAPK = !!window.__GFC_NATIVE_APP__;
        const hasAndroidBridge = !!window.Android;

        if (!isNativeAPK && !hasAndroidBridge && window.Notification && Notification.permission !== 'granted') {
            console.log('[GFC] Browser-only permission sync...');
            window.__gfcNotifDebug?.('CALL: Notification.requestPermission() (BROWSER)');
            await Notification.requestPermission();
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
    },

    /**
     * NATIVE APK CALLBACK: Called by Android after requestNotificationPermission
     * This allows a "One-Click" flow where permission grant automatically triggers registration.
     */
    onNativePermissionResult: async function (granted) {
        window.__gfcNotifDebug?.(`CALLBACK: onNativePermissionResult(granted=${granted})`);
        console.log('[GFC-Notif] Native Callback. Granted:', granted);
        const dotNetRef = window._gfcDotNetRef;
        const vapidKey = window._gfcVapidKey;

        if (granted && vapidKey) {
            try {
                // [FIX] APK mode: NEVER call Notification.requestPermission()
                const subJson = await window.GFC_Notifications.subscribe(vapidKey);
                if (dotNetRef) await dotNetRef.invokeMethodAsync('OnSubscriptionSuccess', subJson);
            } catch (err) {
                console.error('[GFC-Notif] Post-callback error:', err);
                if (dotNetRef) await dotNetRef.invokeMethodAsync('OnSubscriptionError', err.message);
            }
        } else {
            if (dotNetRef) await dotNetRef.invokeMethodAsync('OnPermissionResult', granted ? 'granted' : 'denied');
        }
    }
};

// End of GFC_Notifications
