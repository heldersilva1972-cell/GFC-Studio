// Empty service worker to prevent 404 errors during development
self.addEventListener('install', () => {
    self.skipWaiting();
});

self.addEventListener('activate', () => {
    return self.clients.claim();
});
