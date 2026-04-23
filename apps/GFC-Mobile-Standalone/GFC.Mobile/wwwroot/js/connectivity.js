// GFC Connectivity + Offline Store helpers
// Invoked via Blazor JS Interop

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

    _notify: function (isOnline) {
        if (this._dotnetRef) {
            this._dotnetRef.invokeMethodAsync('OnConnectivityChanged', isOnline);
        }
    },

    dispose: function () {
        this._dotnetRef = null;
    }
};

// Lightweight outbox helpers using localStorage
window.GfcOutbox = {
    _key: 'gfc_mobile_outbox',

    getAll: function () {
        try {
            const raw = localStorage.getItem(this._key);
            return raw ? JSON.parse(raw) : [];
        } catch { return []; }
    },

    add: function (entry) {
        const all = this.getAll();
        all.push(entry);
        localStorage.setItem(this._key, JSON.stringify(all));
    },

    remove: function (id) {
        const all = this.getAll().filter(e => e.id !== id);
        localStorage.setItem(this._key, JSON.stringify(all));
    },

    incrementAttempts: function (id) {
        const all = this.getAll();
        const entry = all.find(e => e.id === id);
        if (entry) {
            entry.attempts = (entry.attempts || 0) + 1;
            localStorage.setItem(this._key, JSON.stringify(all));
        }
    },

    count: function () {
        return this.getAll().length;
    },

    clear: function () {
        localStorage.removeItem(this._key);
    }
};
