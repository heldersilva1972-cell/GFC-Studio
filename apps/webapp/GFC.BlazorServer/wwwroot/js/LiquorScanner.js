// [NEW] Liquor Scanner JS Integration
// Uses html5-qrcode for high-performance barcode scanning

var html5QrCode;

window.LiquorScanner = {
    start: async function (dotNetHelper) {
        // 1. Ensure the library is loaded
        if (typeof Html5Qrcode === "undefined") {
            await this.loadScript("https://unpkg.com/html5-qrcode");
        }

        const config = { 
            fps: 10, 
            qrbox: { width: 250, height: 250 },
            aspectRatio: 1.0
        };

        html5QrCode = new Html5Qrcode("reader");

        try {
            await html5QrCode.start(
                { facingMode: "environment" }, 
                config,
                (decodedText) => {
                    // Success callback
                    dotNetHelper.invokeMethodAsync('OnScanSuccess', decodedText);
                },
                (errorMessage) => {
                    // console.log(errorMessage); // silent errors for noise reduction
                }
            );
        } catch (err) {
            console.error("Unable to start scanner", err);
        }
    },

    stop: async function () {
        if (html5QrCode && html5QrCode.isScanning) {
            await html5QrCode.stop();
            html5QrCode.clear();
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
