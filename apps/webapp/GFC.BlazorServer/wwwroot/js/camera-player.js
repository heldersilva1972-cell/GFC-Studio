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

            hls.on(Hls.Events.ERROR, (event, data) => {
                console.error('HLS Error:', data);
                if (data.fatal) {
                    switch(data.type) {
                        case Hls.ErrorTypes.NETWORK_ERROR:
                            console.log('Network error, attempting recovery...');
                            hls.startLoad();
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
    }
};