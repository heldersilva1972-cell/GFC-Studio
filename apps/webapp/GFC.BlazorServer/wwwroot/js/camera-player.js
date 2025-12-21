// [NEW]
window.CameraPlayer = {
    init: function (videoId, streamUrl) {
        var video = document.getElementById(videoId);
        if (Hls.isSupported()) {
            var hls = new Hls();
            hls.loadSource(streamUrl);
            hls.attachMedia(video);
            hls.on(Hls.Events.MANIFEST_PARSED, function () {
                video.play();
                updateStreamStatus(videoId, 'LIVE', '🟢');
            });
            hls.on(Hls.Events.ERROR, function (event, data) {
                if (data.fatal) {
                    switch (data.type) {
                        case Hls.ErrorTypes.NETWORK_ERROR:
                            console.error('fatal network error encountered, trying to recover');
                            hls.startLoad();
                            break;
                        case Hls.ErrorTypes.MEDIA_ERROR:
                            console.error('fatal media error encountered, trying to recover');
                            hls.recoverMediaError();
                            break;
                        default:
                            console.error('unrecoverable error', data);
                            hls.destroy();
                            updateStreamStatus(videoId, 'OFFLINE', '🔴');
                            break;
                    }
                }
            });
            hls.on(Hls.Events.BUFFER_APPENDING, function () {
                updateStreamStatus(videoId, 'BUFFERING', '⏸️');
            });
            hls.on(Hls.Events.BUFFER_EOS, function () {
                updateStreamStatus(videoId, 'LIVE', '🟢');
            });

        } else if (video.canPlayType('application/vnd.apple.mpegurl')) {
            video.src = streamUrl;
            video.addEventListener('loadedmetadata', function () {
                video.play();
                updateStreamStatus(videoId, 'LIVE', '🟢');
            });
        }
    }
};

function updateStreamStatus(videoId, status, indicator) {
    var videoElement = document.getElementById(videoId);
    if (videoElement) {
        var statusElement = videoElement.parentElement.querySelector('.stream-status');
        if (statusElement) {
            statusElement.innerHTML = `${indicator} ${status}`;
        }
    }
}
