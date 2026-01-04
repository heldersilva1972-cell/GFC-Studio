// GFC PWA Service Worker
// Minimal implementation required for PWA installability
// Does NOT cache aggressively to avoid breaking Blazor Server SignalR

const CACHE_NAME = 'gfc-pwa-v1';
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

// Fetch event - Network-first strategy to avoid breaking Blazor Server
self.addEventListener('fetch', (event) => {
    // Skip caching for:
    // - SignalR connections (_blazor)
    // - API calls
    // - POST/PUT/DELETE requests
    if (
        event.request.url.includes('/_blazor') ||
        event.request.url.includes('/api/') ||
        event.request.method !== 'GET'
    ) {
        return; // Let the browser handle it normally
    }

    // Network-first for everything else
    event.respondWith(
        fetch(event.request)
            .then((response) => {
                // Only cache successful responses
                if (response && response.status === 200) {
                    const responseClone = response.clone();
                    caches.open(CACHE_NAME).then((cache) => {
                        cache.put(event.request, responseClone);
                    });
                }
                return response;
            })
            .catch(() => {
                // Fallback to cache only if network fails
                return caches.match(event.request);
            })
    );
});
