// TEST: Open browser console and paste this to test analytics API

// Test 1: Check if analytics tracker loaded
console.log('Analytics tracker loaded:', typeof window.GFC_Analytics !== 'undefined');

// Test 2: Try to send a test event
fetch('/api/analytics/track', {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({
        action: 'TEST',
        pageUrl: '/test',
        details: 'Manual test from console',
        durationSeconds: 0
    })
})
    .then(response => {
        console.log('API Response Status:', response.status);
        if (response.status === 401) {
            console.error('UNAUTHORIZED - Device token not found or invalid');
        } else if (response.ok) {
            console.log('SUCCESS - Analytics API is working!');
        } else {
            console.error('ERROR - Status:', response.status);
        }
        return response.text();
    })
    .then(text => console.log('Response body:', text))
    .catch(error => console.error('Fetch error:', error));

// Test 3: Check cookies
console.log('Device token cookie:', document.cookie.includes('GFC_DeviceTrustToken') ? 'PRESENT' : 'MISSING');
