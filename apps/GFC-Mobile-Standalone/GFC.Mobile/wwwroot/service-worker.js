// GFC Mobile Revision: 2.1.67
// GFC Mobile - Service Worker
// Cleaned: No-op fetch handler removed to eliminate middleware overhead.

self.addEventListener('install', (event) => {
    self.skipWaiting();
});

self.addEventListener('activate', (event) => {
    event.waitUntil(self.clients.claim());
});
