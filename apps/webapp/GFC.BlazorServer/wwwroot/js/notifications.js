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
    /**
     * UNIFIED BINDING: Binds a click listener to the subscription button.
     * This ensures "User Gesture" validity for both Browser Permission Prompts and Native Bridge calls.
     * (Kept name 'bindNativePrompt' for cache compatibility with C# calls)
     */
    bindNativePrompt: function (buttonId, vapidKey, dotNetRef, retryLimit = 5) {
        const btn = document.getElementById(buttonId);

        if (!btn) {
            if (retryLimit > 0) {
                // console.log(`[GFC] Button ${buttonId} not found, retrying... (${retryLimit} left)`);
                setTimeout(() => this.bindNativePrompt(buttonId, vapidKey, dotNetRef, retryLimit - 1), 250);
            } else {
                console.warn(`[GFC] Failed to find button ${buttonId} after retries.`);
            }
            return;
        }

        // Store globals for callback access if needed
        window._gfcVapidKey = vapidKey;
        window._gfcDotNetRef = dotNetRef;

        // 1. Kill any existing listener to prevent stacking/duplication
        btn.onclick = null;

        // 2. Attach the ONE TRUE HANDLER
        btn.onclick = async function (e) {
            // Execution Gate to prevent double-clicks
            const now = Date.now();
            if (window.__GFC_GATE__ && (now - window.__GFC_GATE__ < 2000)) return;
            window.__GFC_GATE__ = now;

            e.preventDefault();
            e.stopImmediatePropagation();

            // UI Feedback
            const originalContent = btn.innerHTML;
            btn.disabled = true;
            btn.innerHTML = '<span class="spinner-border spinner-border-sm me-2"></span>Wait...';

            let bridgeHandoff = false;

            try {
                const isNativeAPK = !!window.__GFC_NATIVE_APP__;

                // --- PATH A: Native Android Bridge ---
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
                        window.Android.requestNotificationPermission();

                        // Safety timeout to re-enable button if prompt is active
                        setTimeout(() => {
                            if (btn) { btn.disabled = false; btn.innerHTML = originalContent; }
                            window.__GFC_GATE__ = 0;
                        }, 5000);
                        return;
                    }
                }

                // --- PATH B: Standard Browser / PWA ---
                // We are initiating this FROM A CLICK, so requestPermission() is allowed.
                if (!isNativeAPK && window.Notification && Notification.permission === 'default') {
                    console.log('[GFC] Browser Permission Request...');
                    const result = await Notification.requestPermission();
                    if (result !== 'granted') {
                        throw new Error('Notification permission was ' + result);
                    }
                }

                // Proceed to Subscribe (Service Worker)
                const subJson = await window.GFC_Notifications.subscribe(vapidKey);
                if (dotNetRef) await dotNetRef.invokeMethodAsync('OnSubscriptionSuccess', subJson);

            } catch (err) {
                console.error('[GFC] Flow Error:', err);
                if (dotNetRef) await dotNetRef.invokeMethodAsync('OnSubscriptionError', err.message || "Unknown error");
            } finally {
                // If we handed off to the bridge, the Native Callback will handle the UI reset.
                // Otherwise (Browser flow), we reset it here.
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
        if (!vapidPublicKey) throw new Error('VAPID security key is missing. Please check System Settings.');

        const isNativeAPK = !!window.__GFC_NATIVE_APP__;
        const hasAndroidBridge = !!window.Android;

        // 1. FORCED PERMISSION CHECK (Browser-only)
        // If we are in a browser and don't have permission, we MUST call this directly on the click thread
        if (!isNativeAPK && !hasAndroidBridge && window.Notification && Notification.permission === 'default') {
            console.log('[GFC] Requesting browser notification permission...');
            const result = await Notification.requestPermission();
            if (result !== 'granted') {
                throw new Error('Notification permission was ' + result);
            }
        }

        if (!('serviceWorker' in navigator)) {
            throw new Error('This browser does not support background notifications (Service Workers).');
        }

        try {
            // 2. GET REGISTRATION
            // Use getRegistration() as a fallback if ready hangs
            let registration = await navigator.serviceWorker.ready;

            if (!registration) {
                registration = await navigator.serviceWorker.getRegistration();
            }

            if (!registration) {
                throw new Error('Service Worker not registered. Try refreshing the page.');
            }

            // 3. CLEANUP & SUBSCRIBE
            const existing = await registration.pushManager.getSubscription();
            if (existing) await existing.unsubscribe();

            const convertedVapidKey = this.urlBase64ToUint8Array(vapidPublicKey);

            const subscription = await registration.pushManager.subscribe({
                userVisibleOnly: true,
                applicationServerKey: convertedVapidKey
            });

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
