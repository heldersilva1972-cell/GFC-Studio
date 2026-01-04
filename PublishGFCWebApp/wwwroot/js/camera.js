window.cameraCapture = {
    capturePhoto: async function () {
        try {
            if (!navigator.mediaDevices || !navigator.mediaDevices.getUserMedia) {
                throw new Error('Camera API not supported on this device');
            }

            const stream = await navigator.mediaDevices.getUserMedia({ 
                video: { facingMode: 'environment' } // Use back camera if available
            });

            const video = document.createElement('video');
            video.srcObject = stream;
            video.play();

            return new Promise((resolve, reject) => {
                const canvas = document.createElement('canvas');
                const ctx = canvas.getContext('2d');

                video.addEventListener('loadedmetadata', () => {
                    canvas.width = video.videoWidth;
                    canvas.height = video.videoHeight;
                    ctx.drawImage(video, 0, 0);

                    // Stop the stream
                    stream.getTracks().forEach(track => track.stop());

                    // Convert to blob
                    canvas.toBlob((blob) => {
                        if (blob) {
                            const reader = new FileReader();
                            reader.onloadend = () => {
                                const base64data = reader.result;
                                resolve({
                                    success: true,
                                    data: base64data,
                                    fileName: `receipt_${Date.now()}.jpg`,
                                    mimeType: 'image/jpeg'
                                });
                            };
                            reader.onerror = reject;
                            reader.readAsDataURL(blob);
                        } else {
                            reject(new Error('Failed to create blob from canvas'));
                        }
                    }, 'image/jpeg', 0.9);
                });

                video.addEventListener('error', (e) => {
                    stream.getTracks().forEach(track => track.stop());
                    reject(e);
                });
            });
        } catch (error) {
            return {
                success: false,
                error: error.message
            };
        }
    },

    isSupported: function () {
        return !!(navigator.mediaDevices && navigator.mediaDevices.getUserMedia);
    }
};
