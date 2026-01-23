// [NEW] Liquor Scanner JS Integration
// Uses html5-qrcode for high-performance barcode scanning

var html5QrCode;

window.LiquorScanner = {
    start: async function (dotNetHelper) {
        if (typeof Html5Qrcode === "undefined") {
            await this.loadScript("https://unpkg.com/html5-qrcode");
        }

        const config = {
            fps: 10,
            qrbox: { width: 250, height: 250 },
            aspectRatio: 1.0
        };

        const element = document.getElementById("reader");
        if (!element) {
            console.error("Scanner element #reader not found");
            return;
        }

        if (html5QrCode) {
            try { await this.stop(); } catch (e) { }
        }

        html5QrCode = new Html5Qrcode("reader");

        try {
            await html5QrCode.start(
                { facingMode: "environment" },
                config,
                (decodedText) => {
                    dotNetHelper.invokeMethodAsync('OnScanSuccess', decodedText);
                }
            );
        } catch (err) {
            console.error("Unable to start scanner", err);
        }
    },

    stop: async function () {
        if (html5QrCode) {
            if (html5QrCode.isScanning) {
                try { await html5QrCode.stop(); } catch (e) { }
            }
            try { html5QrCode.clear(); } catch (e) { }
            html5QrCode = null;
        }
    },

    startModal: async function (dotNetHelper) {
        if (typeof Html5Qrcode === "undefined") {
            await this.loadScript("https://unpkg.com/html5-qrcode");
        }

        const config = {
            fps: 10,
            qrbox: { width: 250, height: 150 },
            aspectRatio: 1.0
        };

        if (window.modalScanner) {
            try { await window.modalScanner.stop(); } catch (e) { }
        }

        window.modalScanner = new Html5Qrcode("modal-reader");

        try {
            await window.modalScanner.start(
                { facingMode: "environment" },
                config,
                (decodedText) => {
                    dotNetHelper.invokeMethodAsync('OnModalScanSuccess', decodedText);
                }
            );
        } catch (err) {
            console.error("Modal scanner error", err);
        }
    },

    stopModal: async function () {
        if (window.modalScanner && window.modalScanner.isScanning) {
            try { await window.modalScanner.stop(); } catch (e) { }
        }
    },

    stopAllCameras: async function () {
        console.log("LiquorScanner: Stopping all cameras...");
        try {
            // 1. Stop html5-qrcode instances
            if (window.modalScanner) {
                if (window.modalScanner.isScanning) await window.modalScanner.stop().catch(() => { });
                window.modalScanner.clear();
                window.modalScanner = null;
            }
            if (html5QrCode) {
                if (html5QrCode.isScanning) await html5QrCode.stop().catch(() => { });
                html5QrCode.clear();
                html5QrCode = null;
            }

            // 2. Kill all video tracks on the page
            const videos = document.querySelectorAll('video');
            videos.forEach(v => {
                if (v.srcObject) {
                    const tracks = v.srcObject.getTracks();
                    tracks.forEach(track => {
                        track.stop();
                        console.log("Stopped track:", track.label);
                    });
                    v.srcObject = null;
                }
            });

            // 3. Fallback: Global tracks
            if (window.localStream) {
                window.localStream.getTracks().forEach(t => t.stop());
                window.localStream = null;
            }
        } catch (e) {
            console.error("Error stopping cameras:", e);
        }
    },

    getLocalPreview: function (selector) {
        // Try exact ID first, then general selector
        let input = document.getElementById(selector);
        if (!input) input = document.querySelector(selector);

        if (input && input.files && input.files[0]) {
            return URL.createObjectURL(input.files[0]);
        }
        return null;
    },

    revokePreview: function (url) {
        if (url && url.startsWith('blob:')) {
            URL.revokeObjectURL(url);
        }
    },

    loadScript: function (url) {
        return new Promise((resolve, reject) => {
            const script = document.createElement("script");
            script.src = url;
            script.onload = resolve;
            script.onerror = reject;
            document.head.appendChild(script);
        });
    }
};
