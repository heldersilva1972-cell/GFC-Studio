/**
 * GFC POS Offline Sync Engine
 * Uses Dexie.js (IndexedDB) for resilient local storage
 */

if (typeof Dexie !== 'undefined') {
    const db = new Dexie("GfcPosDB");

    // Define Schema: Sales and Products
    db.version(1).stores({
        sales: '++id, timestamp, total, status', // status: 'pending' | 'synced'
        products: 'id, name, price, category'
    });

    window.GfcPos = {
        // 1. Initialize local product cache
        syncProducts: async (products) => {
            try {
                await db.products.clear();
                await db.products.bulkAdd(products);
                console.log("[POS-Sync] Product cache updated:", products.length);
                return true;
            } catch (e) {
                console.error("[POS-Sync] Product sync failed:", e);
                return false;
            }
        },

        // 2. Save sale locally (even if offline)
        saveSale: async (saleData) => {
            try {
                const entry = {
                    ...saleData,
                    timestamp: Date.now(),
                    status: 'pending'
                };
                const id = await db.sales.add(entry);
                console.log("[POS-Offline] Sale saved locally with ID:", id);
                
                // Try to trigger sync if online
                if (navigator.onLine) {
                    // This will be called from Blazor periodically or on-demand
                }
                
                return id;
            } catch (e) {
                console.error("[POS-Offline] Failed to save sale:", e);
                throw e;
            }
        },

        // 3. Get pending sales count
        getPendingCount: async () => {
            return await db.sales.where('status').equals('pending').count();
        },

        // 4. Trigger Sync to Server
        triggerSync: async (dotNetObj) => {
            if (!navigator.onLine) return 0;

            const pending = await db.sales.where('status').equals('pending').toArray();
            if (pending.length === 0) return 0;

            console.log("[POS-Sync] Pushing", pending.length, "sales to server...");
            
            // We'll call back into Blazor to handle the actual HTTP/EF upload
            if (dotNetObj) {
                try {
                    const successCount = await dotNetObj.invokeMethodAsync('ProcessOfflineSales', pending);
                    
                    if (successCount > 0) {
                        // Mark as synced or delete (depending on policy)
                        const syncedIds = pending.slice(0, successCount).map(s => s.id);
                        await db.sales.where('id').anyOf(syncedIds).modify({ status: 'synced' });
                        console.log("[POS-Sync] Successfully synced", successCount, "sales.");
                        return successCount;
                    }
                } catch (err) {
                    console.error("[POS-Sync] Sync callback error:", err);
                }
            }
            return 0;
        }
    };
} else {
    console.warn("[GFC] Dexie.js not found. POS Offline mode will be limited.");
}
