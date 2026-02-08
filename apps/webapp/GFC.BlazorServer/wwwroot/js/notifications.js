window.GFC_Notifications = {
    requestPermission: async function () {
        if (!('Notification' in window)) {
            console.error('Push notifications are not supported in this browser.');
            return 'not_supported';
        }

        if (window.isSecureContext === false) {
            console.error('Push notifications require a secure context (HTTPS or localhost).');
            return 'insecure_context';
        }

        try {
            // Check current status before requesting
            if (Notification.permission === 'granted' || Notification.permission === 'denied') {
                return Notification.permission;
            }

            const permission = await Notification.requestPermission();
            return permission;
        } catch (error) {
            console.error('Error requesting notification permission:', error);
            return 'error';
        }
    },

    getSubscription: async function () {
        const registration = await navigator.serviceWorker.ready;
        const subscription = await registration.pushManager.getSubscription();
        return subscription;
    },

    subscribe: async function (vapidPublicKey) {
        try {
            const registration = await navigator.serviceWorker.ready;

            // Convert VAPID key to Uint8Array
            const padding = '='.repeat((4 - vapidPublicKey.length % 4) % 4);
            const base64 = (vapidPublicKey + padding).replace(/\-/g, '+').replace(/_/g, '/');
            const rawData = window.atob(base64);
            const outputArray = new Uint8Array(rawData.length);
            for (let i = 0; i < rawData.length; ++i) {
                outputArray[i] = rawData.charCodeAt(i);
            }

            const subscription = await registration.pushManager.subscribe({
                userVisibleOnly: true,
                applicationServerKey: outputArray
            });

            return JSON.stringify(subscription);
        } catch (error) {
            console.error('Push subscription failed:', error);
            throw error;
        }
    },

    unsubscribe: async function () {
        const registration = await navigator.serviceWorker.ready;
        const subscription = await registration.pushManager.getSubscription();
        if (subscription) {
            await subscription.unsubscribe();
            return true;
        }
        return false;
    }
};
