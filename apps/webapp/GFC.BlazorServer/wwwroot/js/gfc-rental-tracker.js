/**
 * GFC Rental Telemetry & Analytics Tracker
 * Automatically pings backend /api/rental-tracking/ping on page load,
 * tracks engagement time, wizard step progressions, and button interactions.
 */
(function () {
    'use strict';

    window.GfcRentalTracker = {
        init: function (pageName, isEmbedded) {
            var startTime = Date.now();
            var sessionId = 'gfc_sess_' + Math.random().toString(36).substring(2, 11) + '_' + Date.now();
            var currentStep = 1;

            function sendPing(action, step, didSubmit) {
                var duration = Math.round((Date.now() - startTime) / 1000);
                var payload = {
                    sessionId: sessionId,
                    page: pageName,
                    pageUrl: window.location.href,
                    isEmbedded: !!isEmbedded,
                    referrer: document.referrer || '',
                    step: step || currentStep,
                    durationSeconds: duration,
                    action: action || null,
                    didSubmit: !!didSubmit
                };

                try {
                    if (navigator.sendBeacon) {
                        var blob = new Blob([JSON.stringify(payload)], { type: 'application/json' });
                        navigator.sendBeacon('/api/rental-tracking/ping', blob);
                    } else {
                        fetch('/api/rental-tracking/ping', {
                            method: 'POST',
                            headers: { 'Content-Type': 'application/json' },
                            body: JSON.stringify(payload),
                            keepalive: true
                        }).catch(function () {});
                    }
                } catch (e) {}
            }

            // Initial page load ping
            setTimeout(function () {
                sendPing('Loaded ' + pageName, 1, false);
            }, 300);

            // Periodic heartbeat every 30 seconds to capture session duration accurately
            var heartbeatTimer = setInterval(function () {
                sendPing(null, currentStep, false);
            }, 30000);

            // Send exit ping on beforeunload
            window.addEventListener('beforeunload', function () {
                sendPing('Left Page', currentStep, false);
            });

            // Expose action logger globally
            window.gfcTrackAction = function (actionName) {
                sendPing(actionName, currentStep, false);
            };

            window.gfcTrackStep = function (stepNumber) {
                currentStep = stepNumber;
                sendPing('Step ' + stepNumber + ' Reached', stepNumber, false);
            };

            window.gfcTrackSubmission = function () {
                if (heartbeatTimer) {
                    clearInterval(heartbeatTimer);
                    heartbeatTimer = null;
                }
                sendPing('Application Submitted', 4, true);
            };
        }
    };
})();
