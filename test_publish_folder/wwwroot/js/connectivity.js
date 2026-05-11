// GFC POS Connectivity + Outbox helpers
window.GfcConnectivity = {
    _dotnetRef: null,

    initialize: function (dotnetRef) {
        this._dotnetRef = dotnetRef;
        window.addEventListener('online',  () => this._notify(true));
        window.addEventListener('offline', () => this._notify(false));
    },

    isOnline: function () {
        return navigator.onLine;
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

