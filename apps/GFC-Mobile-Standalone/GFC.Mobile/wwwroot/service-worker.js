// GFC Mobile Revision: 2.1.92 (Cache-Bust Version)
// GFC Mobile - Service Worker

self.addEventListener('install', (event) => {
    console.log('[GFC SW] Install event');
    self.skipWaiting();
});

self.addEventListener('activate', (event) => {
    console.log('[GFC SW] Activate event');
    event.waitUntil(self.clients.claim());
});

// [CRITICAL PWA REQUIREMENT]
// Must have a fetch handler. A no-op is sufficient for installation.
self.addEventListener('fetch', (event) => {
    // Let the network handle it normally
});
