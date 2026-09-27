// GFC Mobile - Background Persistence Worker
// Handles all JSON serialization and IndexedDB writes off the Main UI Thread.

const DB_NAME = "GfcMobileDB";
const STORE_NAME = "PersistenceStore";
const DB_VERSION = 1;

let db = null;

// Initialize DB
const initDB = () => {
    return new Promise((resolve, reject) => {
        const request = indexedDB.open(DB_NAME, DB_VERSION);
        request.onupgradeneeded = (e) => {
            const db = e.target.result;
            if (!db.objectStoreNames.contains(STORE_NAME)) {
                db.createObjectStore(STORE_NAME);
            }
        };
        request.onsuccess = (e) => {
            db = e.target.result;
            resolve(db);
        };
        request.onerror = (e) => reject(e.target.error);
    });
};

self.onmessage = async (e) => {
    if (!db) await initDB();
    
    const { action, key, value } = e.data;
    
    switch (action) {
        case "set": {
            const tx = db.transaction(STORE_NAME, "readwrite");
            const store = tx.objectStore(STORE_NAME);
            const json = typeof value === "string" ? value : JSON.stringify(value);
            const request = store.put(json, key);
            request.onsuccess = () => {
                self.postMessage({ action: "set_result", key, status: "success" });
            };
            break;
        }
            
        case "get": {
            const tx = db.transaction(STORE_NAME, "readonly");
            const store = tx.objectStore(STORE_NAME);
            const request = store.get(key);
            request.onsuccess = () => {
                self.postMessage({ action: "get_result", key, value: request.result });
            };
            request.onerror = () => {
                self.postMessage({ action: "get_result", key, value: null, error: "IndexedDB Read Failed" });
            };
            break;
        }
            
        case "remove": {
            const tx = db.transaction(STORE_NAME, "readwrite");
            const store = tx.objectStore(STORE_NAME);
            store.delete(key);
            break;
        }
            
        case "getAll": {
            const tx = db.transaction(STORE_NAME, "readonly");
            const store = tx.objectStore(STORE_NAME);
            const all = [];
            const request = store.openCursor();
            request.onsuccess = (e) => {
                const cursor = e.target.result;
                if (cursor) {
                    try {
                        let parsedData = cursor.value;
                        if (typeof cursor.value === "string") {
                            try {
                                parsedData = JSON.parse(cursor.value);
                            } catch {
                                parsedData = cursor.value;
                            }
                        }
                        all.push({ key: cursor.key, data: parsedData });
                    } catch (itemErr) {
                        console.warn("[GFC WORKER] Failed to parse item", cursor.key, itemErr);
                    }
                    cursor.continue();
                } else {
                    self.postMessage({ action: "getAll_result", value: all });
                }
            };
            request.onerror = (e) => {
                console.error("[GFC WORKER] getAll cursor error:", e.target.error);
                self.postMessage({ action: "getAll_result", value: all });
            };
            break;
        }
    }
};

console.log("[GFC WORKER] Persistence Service Initialized");
