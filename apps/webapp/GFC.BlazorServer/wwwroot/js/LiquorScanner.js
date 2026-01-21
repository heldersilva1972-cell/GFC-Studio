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
