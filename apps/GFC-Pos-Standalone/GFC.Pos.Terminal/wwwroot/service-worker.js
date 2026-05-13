// GFC POS Standalone Revision: 2.38.60
// In development, always fetch from the network and do not enable caching.
// This is appropriate during development as it avoids complex issues when changing static assets.
self.addEventListener('install', event => event.waitUntil(self.skipWaiting()));
self.addEventListener('activate', event => event.waitUntil(self.clients.claim()));
self.addEventListener('fetch', event => { });










































