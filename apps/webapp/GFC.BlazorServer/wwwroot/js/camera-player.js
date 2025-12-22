// [NEW]
window.CameraPlayer = {
    init: function(videoElementId, streamUrl) {
        const video = document.getElementById(videoElementId);

        if (!video) {
            console.error(`Video element ${videoElementId} not found`);
            return null;
        }

        if (Hls.isSupported()) {
            const hls = new Hls({
                enableWorker: true,
                lowLatencyMode: true,
                backBufferLength: 90,
                maxBufferLength: 30,
                maxMaxBufferLength: 60
            });

            hls.loadSource(streamUrl);
            hls.attachMedia(video);

            hls.on(Hls.Events.MANIFEST_PARSED, () => {
                console.log('HLS manifest parsed, starting playback');
                video.play().catch(e => console.error('Autoplay failed:', e));
            });

            let retryCount = 0;
            const maxRetries = 5;

            hls.on(Hls.Events.ERROR, (event, data) => {
                console.error('HLS Error:', data);
                if (data.fatal) {
                    switch (data.type) {
                        case Hls.ErrorTypes.NETWORK_ERROR:
                            if (retryCount < maxRetries) {
                                const delay = Math.pow(2, retryCount) * 1000;
                                console.log(`Network error, retrying in ${delay}ms...`);
                                setTimeout(() => hls.startLoad(), delay);
                                retryCount++;
                            } else {
                                console.error('Max retries reached. Stopping stream.');
                                hls.destroy();
                            }
                            break;
                        case Hls.ErrorTypes.MEDIA_ERROR:
                            console.log('Media error, attempting recovery...');
                            hls.recoverMediaError();
                            break;
                        default:
                            console.error('Fatal error, destroying HLS instance');
                            hls.destroy();
                            break;
                    }
                }
            });

            return hls;
        } else if (video.canPlayType('application/vnd.apple.mpegurl')) {
            // Safari native HLS support
            video.src = streamUrl;
            video.addEventListener('loadedmetadata', () => {
                video.play().catch(e => console.error('Autoplay failed:', e));
            });
            return video;
        } else {
            console.error('HLS is not supported in this browser');
            return null;
        }
    },

    destroy: function(playerInstance) {
        if (playerInstance && playerInstance.destroy) {
            playerInstance.destroy();
        }
    },

    setQuality: function(playerInstance, streamUrl) {
        if (playerInstance && playerInstance.destroy) {
            playerInstance.destroy();
        }
        // Allow a short delay for the backend to restart the FFmpeg process
        setTimeout(() => {
            const videoElementId = playerInstance.media.id;
            window.CameraPlayer.init(videoElementId, streamUrl);
        }, 1000);
    },

    captureSnapshot: function(playerId) {
        const video = document.getElementById(playerId);
        if (!video) return null;

        const canvas = document.createElement('canvas');
        canvas.width = video.videoWidth;
        canvas.height = video.videoHeight;
        const ctx = canvas.getContext('2d');
        ctx.drawImage(video, 0, 0, canvas.width, canvas.height);
        return canvas.toDataURL('image/jpeg');
    },

    enterFullscreen: function(playerId) {
        const video = document.getElementById(playerId);
        if (video && video.requestFullscreen) {
            video.requestFullscreen();
        }
    },

    exitFullscreen: function() {
        if (document.exitFullscreen) {
            document.exitFullscreen();
        }
    }
};

window.CameraGrid = {
    refreshAll: function() {
        console.log("RefreshAll is now handled by the Blazor component.");
    },

    enterFullscreen: function(playerId) {
        const videoContainer = document.getElementById(playerId)?.closest('.camera-grid-item');
        if (videoContainer && videoContainer.requestFullscreen) {
            videoContainer.requestFullscreen();
        }
    }
};