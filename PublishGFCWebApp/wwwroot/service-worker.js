// GFC PWA Service Worker
// Minimal implementation required for PWA installability
// Does NOT cache aggressively to avoid breaking Blazor Server SignalR

const CACHE_NAME = 'gfc-pwa-v3';
const STATIC_ASSETS = [
    '/',
    '/manifest.json',
    '/images/pwa-icon-192.png',
    '/images/pwa-icon-512.png'
];

// Install event - cache critical static assets only
self.addEventListener('install', (event) => {
    console.log('[Service Worker] Installing...');
    event.waitUntil(
        caches.open(CACHE_NAME).then((cache) => {
            console.log('[Service Worker] Caching static assets');
            return cache.addAll(STATIC_ASSETS);
        })
    );
    self.skipWaiting();
});

// Activate event - clean up old caches
self.addEventListener('activate', (event) => {
    console.log('[Service Worker] Activating...');
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

    if (!isStaticAsset || url.origin !== self.location.origin) {
        return; // Pass through to browser/network
    }

    // For static assets, try network but fall back to cache
    event.respondWith(
        fetch(event.request)
            .then((response) => {
                if (response && response.status === 200) {
                    const responseClone = response.clone();
                    caches.open(CACHE_NAME).then((cache) => {
                        cache.put(event.request, responseClone);
                    });
                }
                return response;
            })
            .catch(async () => {
                const cachedResponse = await caches.match(event.request);
                return cachedResponse || new Response('Asset not found', { status: 404 });
            })
    );
});
