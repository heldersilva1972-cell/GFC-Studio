// [NEW]
function initializeHlsPlayer(videoElementId, hlsUrl) {
    const video = document.getElementById(videoElementId);
    if (Hls.isSupported()) {
        const hls = new Hls();
        hls.loadSource(hlsUrl);
        hls.attachMedia(video);
        hls.on(Hls.Events.MANIFEST_PARSED, function () {
            video.play();
        });
    } else if (video.canPlayType('application/vnd.apple.mpegurl')) {
        video.src = hlsUrl;
        video.addEventListener('loadedmetadata', function () {
            video.play();
        });
    }
}

function setPlaybackSpeed(videoElementId, speed) {
    const video = document.getElementById(videoElementId);
    video.playbackRate = speed;
}

function seekVideo(videoElementId, percentage) {
    const video = document.getElementById(videoElementId);
    video.currentTime = video.duration * (percentage / 100);
}
