window.LiquorScanner = {
    instances: {},
    scriptPromise: null,

    loadScript: function (url) {
        if (window.Html5Qrcode) return Promise.resolve();
        if (this.scriptPromise) return this.scriptPromise;

        console.log("LiquorScanner: Loading html5-qrcode library...");
        this.scriptPromise = new Promise((resolve, reject) => {
            const script = document.createElement("script");
            script.src = url;
            script.onload = () => {
                console.log("LiquorScanner: Library loaded.");
                resolve();
            };
            script.onerror = (e) => {
                this.scriptPromise = null;
                console.error("LiquorScanner: Library failed to load.", e);
                reject(e);
            };
            document.head.appendChild(script);
        });
        return this.scriptPromise;
    },

    start: async function (elementId, dotNetHelper, callbackName) {
        try {
            console.log(`LiquorScanner: Initializing scanner on #${elementId}`);
            await this.loadScript("https://unpkg.com/html5-qrcode");

            // Stop any existing session first without awaiting indefinitely
            this.stopAllCameras().catch(e => console.warn("LiquorScanner: Pre-start cleanup failed", e));

            const config = {
                fps: 20,
                qrbox: (viewWidth, viewHeight) => {
                    const minDim = Math.min(viewWidth, viewHeight);
                    return { width: minDim * 0.8, height: minDim * 0.8 };
                },
                aspectRatio: 1.0,
                experimentalFeatures: {
                    useBarCodeDetectorIfSupported: true
                }
            };

            const scanner = new Html5Qrcode(elementId);
            this.instances[elementId] = scanner;

            await scanner.start(
                { facingMode: "environment" },
                config,
                (decodedText) => {
                    console.log("LiquorScanner: Valid scan detected:", decodedText);
                    this.playBeep();
                    // Non-blocking call back to Blazor
                    setTimeout(() => {
                        dotNetHelper.invokeMethodAsync(callbackName, decodedText)
                            .catch(err => console.error("LiquorScanner: Failed to notify Blazor", err));
                    }, 10);
                },
                (errorMessage) => { /* Quiet scan noise */ }
            );
            console.log("LiquorScanner: Camera active and scanning.");
        } catch (err) {
            console.error(`LiquorScanner: Critical startup error:`, err);

            let msg = "Camera Access Error";
            if (err && err.name === 'NotAllowedError') msg += ": Permission denied. Please enable camera access.";
            else if (err && err.name === 'NotFoundError') msg += ": No camera found.";
            else if (err && err.message) msg += ": " + err.message;
            else if (typeof err === 'string') msg += ": " + err;
            else msg += ": Unexpected failure.";

            alert(msg);
            throw err;
        }
    },

    stop: async function (elementId) {
        const scanner = this.instances[elementId];
        if (scanner) {
            console.log(`LiquorScanner: Tearing down scanner on #${elementId}`);
            try {
                if (scanner.isScanning) {
                    await scanner.stop();
                }
            } catch (e) {
                console.warn("LiquorScanner: Stop call failed", e);
            }
            try { scanner.clear(); } catch (e) { }
            delete this.instances[elementId];
        }
    },

    stopAllCameras: async function () {
        console.log("LiquorScanner: Emergency global camera stop requested.");

        // 1. Stop known library instances
        const ids = Object.keys(this.instances);
        for (const id of ids) {
            try {
                const scanner = this.instances[id];
                if (scanner) {
                    if (scanner.isScanning) await scanner.stop().catch(() => { });
                    scanner.clear();
                }
            } catch (e) { }
            delete this.instances[id];
        }

        // 2. Clear our internal video track references
        this.activeVideoStream = null;

        // 3. Bruteforce every media track in the browser (Atomic Fix)
        try {
            const videoElements = document.querySelectorAll('video');
            videoElements.forEach(v => {
                const stream = v.srcObject;
                if (stream && stream.getTracks) {
                    stream.getTracks().forEach(t => {
                        console.log("LiquorScanner: Forcibly stopping track:", t.label);
                        t.stop();
                    });
                }
                v.srcObject = null;
                try { v.load(); } catch (e) { }
            });
        } catch (err) {
            console.warn("LiquorScanner: Track cleanup error", err);
        }
    },

    triggerClick: function (id) {
        try {
            const el = document.getElementById(id);
            if (el) {
                console.log("LiquorScanner: Firing click event on", id);
                el.click();
            } else {
                console.error("LiquorScanner: Cannot click missing element:", id);
            }
        } catch (e) {
            console.error("LiquorScanner: Trigger click error", e);
        }
    },

    playBeep: function () {
        try {
            const audioCtx = new (window.AudioContext || window.webkitAudioContext)();
            const oscillator = audioCtx.createOscillator();
            const gainNode = audioCtx.createGain();

            oscillator.connect(gainNode);
            gainNode.connect(audioCtx.destination);

            oscillator.type = 'sine';
            oscillator.frequency.setValueAtTime(880, audioCtx.currentTime);
            gainNode.gain.setValueAtTime(0, audioCtx.currentTime);
            gainNode.gain.linearRampToValueAtTime(0.3, audioCtx.currentTime + 0.05);
            gainNode.gain.linearRampToValueAtTime(0, audioCtx.currentTime + 0.2);

            oscillator.start();
            oscillator.stop(audioCtx.currentTime + 0.2);
        } catch (e) { }
    },

    startLiveCamera: async function (elementId) {
        try {
            const container = document.getElementById(elementId);
            if (!container) return;

            await this.stopAllCameras();

            const stream = await navigator.mediaDevices.getUserMedia({
                video: { facingMode: "environment" },
                audio: false
            }).catch(e => { throw new Error("Could not access camera: " + e.message); });

            const video = document.createElement("video");
            video.srcObject = stream;
            video.setAttribute("playsinline", true);
            video.muted = true;
            video.style.width = "100%";
            video.style.height = "100%";
            video.style.objectFit = "cover";

            container.innerHTML = "";
            container.appendChild(video);

            video.onloadedmetadata = () => video.play().catch(e => console.warn("Play failed", e));

            this.activeVideoStream = stream;
        } catch (err) {
            console.error("LiquorScanner: Live camera error:", err);
            alert("Camera Error: " + err.message);
        }
    },

    takeSnapshot: async function () {
        try {
            const video = document.querySelector("video");
            if (!video || !this.activeVideoStream) {
                console.error("LiquorScanner: No active video stream for snapshot");
                return null;
            }

            const canvas = document.createElement("canvas");
            canvas.width = video.videoWidth || video.clientWidth;
            canvas.height = video.videoHeight || video.clientHeight;

            const ctx = canvas.getContext("2d");
            ctx.drawImage(video, 0, 0, canvas.width, canvas.height);

            const dataUrl = canvas.toDataURL("image/jpeg", 0.8);
            this.playBeep();

            this.stopAllCameras().catch(() => { });

            return dataUrl;
        } catch (err) {
            console.error("LiquorScanner: Snapshot error", err);
            return null;
        }
    }
};
