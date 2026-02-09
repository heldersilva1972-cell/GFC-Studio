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
        if (!('Notification' in window)) {
            console.warn('This browser does not support notifications.');
            return 'denied';
        }

        if (Notification.permission === 'granted') {
            return 'granted';
        }

        try {
            const permission = await Notification.requestPermission();
            return permission;
        } catch (error) {
            console.error('Error requesting notification permission:', error);
            return 'denied';
        }
    },

    /**
     * Subscribes the user to push notifications via the Service Worker
     * @param {string} vapidPublicKey The public VAPID key from the server
     * @returns {Promise<string|null>} JSON string of the subscription object or null
     */
    subscribe: async function (vapidPublicKey) {
        if (!('serviceWorker' in navigator)) {
            throw new Error('Service Worker not supported');
        }

        try {
            const registration = await navigator.serviceWorker.ready;
            
            // Optimization: check for existing subscription first
            let subscription = await registration.pushManager.getSubscription();
            
            if (subscription) {
                // Return existing subscription if it's already there
                return JSON.stringify(subscription);
            }

            // Create new subscription
            const convertedVapidKey = this.urlBase64ToUint8Array(vapidPublicKey);
            
            subscription = await registration.pushManager.subscribe({
                userVisibleOnly: true,
                applicationServerKey: convertedVapidKey
            });

            console.log('[Push] User is subscribed:', subscription.endpoint);
            return JSON.stringify(subscription);
        } catch (error) {
            console.error('[Push] Failed to subscribe the user:', error);
            throw error;
        }
    },

    /**
     * Unsubscribes the user from push notifications
     * @returns {Promise<boolean>} Success status
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
     * @returns {Promise<boolean>}
     */
    getSubscription: async function () {
        if (!('serviceWorker' in navigator)) return false;
        
        const registration = await navigator.serviceWorker.ready;
        const subscription = await registration.pushManager.getSubscription();
        return !!subscription;
    },

    /**
     * Helper: Convert VAPID key to Uint8Array required by pushManager
     */
    urlBase64ToUint8Array: function (base64String) {
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
