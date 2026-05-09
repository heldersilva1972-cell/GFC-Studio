// GFC PWA Service Worker
// Minimal implementation required for PWA installability
// Does NOT cache aggressively to avoid breaking Blazor Server SignalR

const CACHE_NAME = 'gfc-pwa-v13';
const STATIC_ASSETS = [
    '/',
    '/manifest.json',
    '/offline.html',
    '/css/mobile-modern.css',
    '/css/dashboard-modern.css',
    '/app.css',
    '/bootstrap/bootstrap.min.css'
];

// Optional assets that won't block installation if they fail
const OPTIONAL_ASSETS = [
    '/images/pwa-icon-192.png',
    '/images/pwa-icon-512.png'
];

// Install event - cache critical static assets only
self.addEventListener('install', (event) => {
    console.log('[Service Worker v8] Installing...');
    event.waitUntil(
        caches.open(CACHE_NAME).then(async (cache) => {
            console.log('[Service Worker] Caching critical assets');

            // Cache critical assets first
            try {
                await cache.addAll(STATIC_ASSETS);
                console.log('[Service Worker] Critical assets cached successfully');
            } catch (error) {
                console.error('[Service Worker] Failed to cache critical assets:', error);
                // Don't throw - we still want to try to install even if some assets fail
            }
        })
    );
    self.skipWaiting();
});

// Activate event - clean up old caches
self.addEventListener('activate', (event) => {
    console.log('[Service Worker v8] Activating...');
    event.waitUntil(
        caches.keys().then((cacheNames) => {
            return Promise.all(
                cacheNames.map((cacheName) => {
                    if (cacheName !== CACHE_NAME) {
                        console.log('[Service Worker] Deleting old cache:', cacheName);
                        return caches.delete(cacheName);
                    }
                })
            );
        })
    );
    self.clients.claim();
});

// Fetch event - Bypass by default to avoid breaking Blazor Server / SignalR
self.addEventListener('fetch', (event) => {
    const url = new URL(event.request.url);

    // ONLY intercept internal static assets we want to cache
    // Everything else (navigation, SignalR, API, External scripts) passes through to the browser
    const isStaticAsset = STATIC_ASSETS.some(asset => url.pathname === asset || url.pathname.startsWith('/images/'));
    const isNavigation = event.request.mode === 'navigate';

    // 1. Handle Navigation Requests (HTML) - Network First, Offline Fallback
    if (isNavigation) {
        event.respondWith(
            fetch(event.request)
                .catch(() => {
                    return caches.match('/offline.html');
                })
        );
        return;
    }

    // 2. Handle Static Assets - Cache First, Network Fallback
    if (isStaticAsset && url.origin === self.location.origin) {
        event.respondWith(
            caches.match(event.request)
                .then((cachedResponse) => {
                    if (cachedResponse) {
                        return cachedResponse;
                    }
                    return fetch(event.request).then((response) => {
                        // Don't cache here dynamically to avoid bloating cache with unwanted assets
                        return response;
                    });
                })
        );
        return;
    }

    // 3. Default: Network Only for everything else (SignalR, API, etc.)
    return;
});

// Push notification handler
self.addEventListener('push', (event) => {
    console.log('[Service Worker v8] Push Received.');
    let data = { title: 'GFC Alert', body: 'System notification received.' };

    if (event.data) {
        try {
            data = event.data.json();
        } catch (e) {
            data = { title: 'GFC Alert', body: event.data.text() };
        }
    }

    // [PREMIUM UI] Added icon, badge and tag for native app feel
    const options = {
        body: data.body,
        icon: '/images/pwa-icon-192.png',
        badge: '/favicon.png',
        vibrate: [100, 50, 100],
        data: data.url || '/',
        tag: 'gfc-inventory-alert', // Specific tag to group inventory alerts
        // renotify: true,   // REMOVED: This causes Chrome to show a secondary "Tap to Copy URL" notification on Android
        timestamp: Date.now()
    };

    event.waitUntil(
        self.registration.showNotification(data.title, options)
    );
});

// Notification click handler
self.addEventListener('notificationclick', (event) => {
    console.log('[Service Worker v8] Notification click Received.');

    event.notification.close();

    const urlToOpen = event.notification.data || '/';

    event.waitUntil(
        clients.matchAll({ type: 'window', includeUncontrolled: true }).then((windowClients) => {
            for (let i = 0; i < windowClients.length; i++) {
                const client = windowClients[i];
                if (client.url === urlToOpen && 'focus' in client) {
                    return client.focus();
                }
            }
            if (clients.openWindow) {
                return clients.openWindow(urlToOpen);
            }
        })
    );
});
