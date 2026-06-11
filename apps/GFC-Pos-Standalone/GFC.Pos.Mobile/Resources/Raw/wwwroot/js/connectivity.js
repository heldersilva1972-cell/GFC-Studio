// GFC POS Connectivity + Outbox helpers
window.GfcConnectivity = {
    _dotnetRef: null,

    initialize: function (dotnetRef) {
        this._dotnetRef = dotnetRef;
        window.addEventListener('online',  () => this._notify(true));
        window.addEventListener('offline', () => this._notify(false));
    },

    isOnline: async function () {
        if (!navigator.onLine) {
            return false;
        }
        try {
            const controller = new AbortController();
            const id = setTimeout(() => controller.abort(), 2000);
            await fetch('https://www.google.com/favicon.ico', {
                mode: 'no-cors',
                cache: 'no-store',
                signal: controller.signal
            });
            clearTimeout(id);
            return true;
        } catch (e) {
            return false;
        }
    },

    // [DIAGNOSTIC] Returns detailed state
    getState: function() {
        return {
            onLine: navigator.onLine,
            userAgent: navigator.userAgent,
            timestamp: new Date().toISOString()
        };
    },

    _notify: function (isOnline) {
        if (this._dotnetRef) {
            this._dotnetRef.invokeMethodAsync('OnConnectivityChanged', isOnline);
        }
    },

    dispose: function () {
        this._dotnetRef = null;
    }
};

