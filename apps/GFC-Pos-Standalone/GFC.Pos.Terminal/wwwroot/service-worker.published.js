// GFC POS Revision: 2.43.55
// Caution! Be sure you understand the caveats before using an offline-first
// service worker. See https://aka.ms/blazor-offline-first

self.importScripts('./service-worker-assets.js');
self.addEventListener('install', event => {
    console.info('Service worker: Install (Forced Update)');
    self.skipWaiting(); // FORCE IMMEDIATE ACTIVATION
    event.waitUntil(onInstall(event));
});
self.addEventListener('activate', event => {
    console.info('Service worker: Activate');
    event.waitUntil(self.clients.claim()); // FORCE TAKE CONTROL IMMEDIATELY
    event.waitUntil(onActivate(event));
});
self.addEventListener('fetch', event => event.respondWith(onFetch(event)));

// Revision: 2.43.4
/* cache-name-2.43.4 */
const cacheNamePrefix = 'offline-cache-';
const cacheName = `${cacheNamePrefix}2.43.55`;
const offlineAssetsInclude = [ /\.dll$/, /\.pdb$/, /\.wasm/, /\.html/, /\.js$/, /\.json$/, /\.css$/, /\.woff$/, /\.png$/, /\.jl$/, /\.svg$/, /\.dat$/, /gfc-storage-worker\.js$/ ];
const offlineAssetsExclude = [ /^service-worker\.js$/ ];

async function onInstall(event) {
    console.info('Service worker: Install');

    // Fetch and cache all matching items from the assets manifest
    const assetsRequests = self.assetsManifest.assets
        .filter(asset => offlineAssetsInclude.some(pattern => pattern.test(asset.url)))
        .filter(asset => !offlineAssetsExclude.some(pattern => pattern.test(asset.url)))
        .map(asset => new Request(asset.url, { integrity: asset.hash, cache: 'no-cache' }));
    await caches.open(cacheName).then(cache => cache.addAll(assetsRequests));
}

async function onActivate(event) {
    console.info('Service worker: Activate');

    // Delete unused caches
    const cacheKeys = await caches.keys();
    await Promise.all(cacheKeys
        .filter(key => key.startsWith(cacheNamePrefix) && key !== cacheName)
        .map(key => caches.delete(key)));
}

async function onFetch(event) {
    let cachedResponse = null;
    if (event.request.method === 'GET') {
        const cache = await caches.open(cacheName);
        
        // Handle navigation requests (index.html)
        const request = event.request.mode === 'navigate' ? 'index.html' : event.request;
        cachedResponse = await cache.match(request);
        
        if (cachedResponse) {
            return cachedResponse;
        }
    }

    // Network Fallback
    try {
        return await fetch(event.request);
    } catch (err) {
        return null;
    }
}


















































































































































































