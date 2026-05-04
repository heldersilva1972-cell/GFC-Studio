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

    checkHonestInternet: async function () {
        if (!navigator.onLine) return false;
        try {
            const controller = new AbortController();
            const id = setTimeout(() => controller.abort(), 3000);
            
            // Use a reliable public endpoint with no-cors to avoid preflight blocks
            await fetch('https://www.google.com/generate_204', { 
                mode: 'no-cors',
                cache: 'no-cache',
                signal: controller.signal
            });
            clearTimeout(id);
            return true;
        } catch (e) {
            return false;
        }
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

// ─── INDEXEDDB HANDSHAKE BRIDGE (Vault) ───
// Provides unified storage for large/sensitive outbox data
(function() {
    const DB_NAME = 'GfcVault';
    const STORE_NAME = 'outbox';
    const DB_VERSION = 1;

    function getDB() {
        return new Promise((resolve, reject) => {
            const request = indexedDB.open(DB_NAME, DB_VERSION);
            request.onupgradeneeded = (e) => {
                const db = e.target.result;
                if (!db.objectStoreNames.contains(STORE_NAME)) {
                    db.createObjectStore(STORE_NAME, { keyPath: 'key' });
                }
            };
            request.onsuccess = (e) => resolve(e.target.result);
            request.onerror = (e) => reject(e.target.error);
        });
    }

    window.gfcSetAsync = async function(key, data) {
        try {
            const db = await getDB();
            const tx = db.transaction(STORE_NAME, 'readwrite');
            const store = tx.objectStore(STORE_NAME);
            await store.put({ key: key, data: data, timestamp: Date.now() });
            return true;
        } catch (e) {
            console.error("gfcSetAsync failed:", e);
            return false;
        }
    };

    window.gfcGetAsync = async function(key) {
        try {
            const db = await getDB();
            const tx = db.transaction(STORE_NAME, 'readonly');
            const store = tx.objectStore(STORE_NAME);
            return new Promise((resolve) => {
                const req = store.get(key);
                req.onsuccess = () => resolve(req.result ? JSON.stringify(req.result.data) : null);
                req.onerror = () => resolve(null);
            });
        } catch (e) {
            return null;
        }
    };

    window.gfcRemoveAsync = async function(key) {
        try {
            const db = await getDB();
            const tx = db.transaction(STORE_NAME, 'readwrite');
            await tx.objectStore(STORE_NAME).delete(key);
            return true;
        } catch (e) {
            return false;
        }
    };

    window.gfcGetAllAsync = async function() {
        try {
            const db = await getDB();
            const tx = db.transaction(STORE_NAME, 'readonly');
            const store = tx.objectStore(STORE_NAME);
            return new Promise((resolve) => {
                const req = store.getAll();
                req.onsuccess = () => resolve(req.result);
                req.onerror = () => resolve([]);
            });
        } catch (e) {
            return [];
        }
    };
})();
