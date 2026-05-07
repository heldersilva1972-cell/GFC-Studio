// GFC Mobile Revision: 2.4.5 (Auto-Clear Input Behavior Fix)
self.addEventListener('install', (event) => {
    self.skipWaiting();
});

self.addEventListener('activate', (event) => {
    event.waitUntil(self.clients.claim());
});

self.addEventListener('fetch', (event) => {
    // No-op fetch handler for PWA compliance
});
